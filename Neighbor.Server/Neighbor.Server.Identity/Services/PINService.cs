using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Neighbor.Server.Identity.Data;
using Neighbor.Server.Identity.Data.Entity;
using Neighbor.Server.Identity.Models;
using Neighbor.Server.Identity.Services.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbor.Server.Identity.Services
{
    public class PINService : IPINService
    {
        private readonly IServiceProvider services;

        public PINService(IServiceProvider serviceProvider)
        {
            services = serviceProvider;
        }

        public async Task<GeneratePINResultModel> GeneratePINAsync(string userName, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return new GeneratePINResultModel { IsSuccess = false, Message = "Operation cancel by client." };
            }

            var reference = string.Empty;

            var userManager = (UserManager<IdentityUser>)services.GetService(typeof(UserManager<IdentityUser>));
            var identityUser = await userManager.FindByNameAsync(userName);

            if (identityUser == null)
            {
                return new GeneratePINResultModel { IsSuccess = false, Message = "User not found." };
            }

            using (var identityDbContext = (IdentityDbContext)services.GetService(typeof(IdentityDbContext)))
            {
                var existUserPINEntities = identityDbContext.UserPINs
                    .Where(p =>
                        p.UserId == identityUser.Id
                        && p.EnterOn == null
                        && (p.CreatedOn >= DateTime.Now.AddDays(-1) && p.CreatedOn <= DateTime.Now));

                var hasExistedPIN = existUserPINEntities.Count() >= 3;
                if (hasExistedPIN)
                {
                    await existUserPINEntities.Where(p => p.IsEnable == true).ForEachAsync(entity =>
                    {
                        entity.IsEnable = false;
                    }, cancellationToken);
                    await identityDbContext.SaveChangesAsync(cancellationToken);

                    return new GeneratePINResultModel { IsSuccess = false, Message = "Has many active PINs in system. Please try in next 24 hrs." };
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

        public async Task<VerifyPINResultModel> VerifyPINAsync(string userName, KeyValuePair<string, string> pinAndRef, CancellationToken cancellationToken)
        {
            var resultModel = new VerifyPINResultModel();
            using (var identityDbContext = (IdentityDbContext)services.GetService(typeof(IdentityDbContext)))
            {
                var userManager = (UserManager<IdentityUser>)services.GetService(typeof(UserManager<IdentityUser>));
                var identityUser = await userManager.FindByNameAsync(userName);
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
                    resultModel.Message = "PIN not found";

                    return resultModel;
                }

                if (!userPINEntity.IsEnable)
                {
                    resultModel.Message = "PIN is disabled";

                    if (userPINEntity.EnterOn != null)
                    {
                        resultModel.Message = "PIN is used";
                    }
                    return resultModel;
                }

                var isPINIsUsed = userPINEntity.EnterOn != null;
                if (isPINIsUsed)
                {
                    resultModel.Message = "PIN has used";
                    return resultModel;
                }

                var isPINAlive = DateTime.Now.Subtract(userPINEntity.CreatedOn).TotalMinutes <= 3;
                if (!isPINAlive)
                {
                    userPINEntity.IsEnable = false;                    
                    updateUserPINModelFunc();

                    resultModel.Message = "PIN Expired";
                    return resultModel;
                }

                var isPINValidAttempt = userPINEntity.InvalidAttempt < 3;
                if (!isPINValidAttempt)
                {                    
                    updateUserPINModelFunc();

                    resultModel.Message = "PIN reach max try";
                    return resultModel;
                }

                var isPINValid = userPINEntity.PIN == pinAndRef.Key;
                if (!isPINValid)
                {
                    userPINEntity.InvalidAttempt++;
                    updateUserPINModelFunc();

                    resultModel.Message = $"PIN not match ({userPINEntity.InvalidAttempt})";
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
