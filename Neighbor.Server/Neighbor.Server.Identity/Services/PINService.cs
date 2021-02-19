using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Neighbor.Core.Domain.Models.Security;
using Neighbor.Server.Identity.Data;
using Neighbor.Server.Identity.Data.Entity;
using Neighbor.Server.Identity.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbor.Server.Identity.Services
{
    public class PINService : IPINService
    {
        private readonly IServiceProvider services;

        private Dictionary<string, string> PINErrorDictionary => new Dictionary<string, string>(new[]
        {
            new KeyValuePair<string,string>("PIN0001","Operation cancel by client"),
            new KeyValuePair<string,string>("PIN0002","User not found"),
            new KeyValuePair<string,string>("PIN0003","Has many active PINs in system. Please try in next 24 hrs"),
            new KeyValuePair<string,string>("PIN0004","PIN not found"),
            new KeyValuePair<string,string>("PIN0005","PIN is disabled"),
            new KeyValuePair<string,string>("PIN0006","PIN is used"),
            new KeyValuePair<string,string>("PIN0007","PIN Expired"),
            new KeyValuePair<string,string>("PIN0008","PIN reach max try"),
            new KeyValuePair<string,string>("PIN0009","PIN not match")
        });

        public PINService(IServiceProvider serviceProvider)
        {
            services = serviceProvider;
        }

        public async Task<GeneratePINResultModel> GeneratePINAsync(IdentityUser identityUser, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return new GeneratePINResultModel { IsSuccess = false, Code = "PIN0001", Message = PINErrorDictionary["PIN0001"] };
            }

            var reference = string.Empty;

            if (identityUser == null)
            {
                return new GeneratePINResultModel { IsSuccess = false, Code = "PIN0002", Message = PINErrorDictionary["PIN0002"] };
            }

            using (var identityDbContext = (IdentityDbContext)services.GetService(typeof(IdentityDbContext)))
            {
                var existUserPINEntities = identityDbContext.UserPINs
                    .Where(p =>
                        p.UserId == identityUser.Id
                        && p.IsDelete == false
                        && (p.CreatedOn >= DateTime.Now.AddDays(-1) && p.CreatedOn <= DateTime.Now));

                var hasExistedPIN = existUserPINEntities.Count() >= 3;
                if (hasExistedPIN)
                {
                    await existUserPINEntities.Where(p => p.IsEnable == true).ForEachAsync(entity =>
                    {
                        entity.IsEnable = false;
                    }, cancellationToken);
                    await identityDbContext.SaveChangesAsync(cancellationToken);

                    return new GeneratePINResultModel { IsSuccess = false, Code = "PIN0003", Message = PINErrorDictionary["PIN0003"] };
                }

                var pinStringBuilder = new StringBuilder();
                var refStringBuilder = new StringBuilder();
                var random = new Random();
                var refChars = new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
                for (int i = 0; i < 6; i++)
                {
                    var digit = random.Next(0, 9);
                    var refChar = refChars[digit];

                    pinStringBuilder.Append(digit);
                    refStringBuilder.Append(refChar);
                }

                var pin = pinStringBuilder.ToString();
                reference = refStringBuilder.ToString();

                var userPINModel = new UserPINEntity
                {
                    PIN = pin,
                    Reference = reference,
                    CreatedOn = DateTime.Now,
                    UserId = identityUser.Id,
                    IsEnable = true,
                    ChannelType = "Phone",
                    ChannelAddress = identityUser.PhoneNumber
                };

                await identityDbContext.AddAsync(userPINModel, cancellationToken);

                await existUserPINEntities.ForEachAsync(entity =>
                {
                    entity.IsEnable = false;
                }, cancellationToken);

                await identityDbContext.SaveChangesAsync(cancellationToken);
            }

            return new GeneratePINResultModel { Reference = reference, IsSuccess = true, Message = "Succeeded" };
        }

        public async Task<VerifyPINResultModel> VerifyPINAsync(IdentityUser identityUser, KeyValuePair<string, string> pinAndRef, CancellationToken cancellationToken)
        {
            var resultModel = new VerifyPINResultModel();
            using (var identityDbContext = (IdentityDbContext)services.GetService(typeof(IdentityDbContext)))
            {
                var userPINEntity = await identityDbContext.UserPINs.SingleOrDefaultAsync(p =>
                    p.Reference == pinAndRef.Value &&
                    p.UserId == identityUser.Id, cancellationToken);
                var isPINExisted = userPINEntity != null;
                var updateUserPINModelFunc = new Action(() =>
                {
                    if (userPINEntity.InvalidAttempt == 3)
                    {
                        userPINEntity.IsEnable = false;
                    }

                    identityDbContext.Attach(userPINEntity);
                    identityDbContext.SaveChanges();
                });

                if (!isPINExisted)
                {
                    resultModel.Result = false;
                    resultModel.Code = "PIN0004";
                    resultModel.Message = PINErrorDictionary["PIN0004"];

                    return resultModel;
                }

                if (!userPINEntity.IsEnable)
                {
                    resultModel.Result = false;
                    resultModel.Code = "PIN0005";
                    resultModel.Message = PINErrorDictionary["PIN0005"];

                    if (userPINEntity.EnterOn != null)
                    {
                        resultModel.Result = false;
                        resultModel.Code = "PIN0006";
                        resultModel.Message = PINErrorDictionary["PIN0006"];
                    }
                    return resultModel;
                }

                var isPINIsUsed = userPINEntity.EnterOn != null;
                if (isPINIsUsed)
                {
                    resultModel.Result = false;
                    resultModel.Code = "PIN0006";
                    resultModel.Message = PINErrorDictionary["PIN0006"];
                    return resultModel;
                }

                var isPINAlive = DateTime.Now.Subtract(userPINEntity.CreatedOn).TotalMinutes <= 3;
                if (!isPINAlive)
                {
                    userPINEntity.IsEnable = false;
                    updateUserPINModelFunc();
                    
                    resultModel.Result = false;
                    resultModel.Code = "PIN0007";
                    resultModel.Message = PINErrorDictionary["PIN0007"];
                    return resultModel;
                }

                var isPINValidAttempt = userPINEntity.InvalidAttempt < 3;
                if (!isPINValidAttempt)
                {
                    updateUserPINModelFunc();

                    resultModel.Result = false;
                    resultModel.Code = "PIN0008";
                    resultModel.Message = PINErrorDictionary["PIN0008"];
                    return resultModel;
                }

                var isPINValid = userPINEntity.PIN == pinAndRef.Key;
                if (!isPINValid)
                {
                    userPINEntity.InvalidAttempt++;
                    updateUserPINModelFunc();

                    resultModel.Result = false;
                    resultModel.Code = "PIN0009";
                    resultModel.Message = string.Format("{0} ({1})", PINErrorDictionary["PIN0009"], userPINEntity.InvalidAttempt);
                    return resultModel;
                }

                resultModel.Result = isPINAlive && isPINValidAttempt && isPINValid;

                userPINEntity.EnterOn = DateTime.Now;
                userPINEntity.IsEnable = false;
                updateUserPINModelFunc();
            }

            return resultModel;
        }
    }
}
