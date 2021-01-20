using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neighbor.Core.Domain.Models.Security;
using Neighbor.Server.Identity.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbor.Server.Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PINController : Controller
    {
        private IServiceProvider services;

        public PINController(IServiceProvider serviceProvider)
        {
            services = serviceProvider;
        }

        [Authorize(AuthenticationSchemes = "Basic", Policy = "Basic")]
        [HttpGet]
        [Route("Generate/{phoneNumber}")]
        public async Task<GeneratePINResultModel> Generate(string phoneNumber, CancellationToken cancellationToken)
        {
            var userManager = (UserManager<IdentityUser>)services.GetService(typeof(UserManager<IdentityUser>));
            var userIdentity = await userManager.Users.SingleOrDefaultAsync(p => p.PhoneNumber == phoneNumber, cancellationToken);

            var pinService = (IPINService)services.GetService(typeof(IPINService));
            var pinReference = await pinService.GeneratePINAsync(userIdentity, cancellationToken);

            return pinReference;
        }

        [Authorize]
        [HttpGet]
        [Route("Generate")]
        public async Task<GeneratePINResultModel> Generate(CancellationToken cancellationToken)
        {
            var userManager = (UserManager<IdentityUser>)services.GetService(typeof(UserManager<IdentityUser>));
            var userIdentity = await userManager.FindByNameAsync(User.Identity.Name);

            var pinService = (IPINService)services.GetService(typeof(IPINService));
            var pinReference = await pinService.GeneratePINAsync(userIdentity, cancellationToken);

            return pinReference;
        }

        [Authorize(AuthenticationSchemes = "Basic", Policy = "Basic")]
        [HttpPost]
        [Route("Verify/{phoneNumber}")]
        public async Task<VerifyPINResultModel> Verify(string phoneNumber, [FromForm]IFormCollection forms, CancellationToken cancellationToken)
        {
            var pin = forms["pin"].ToString();
            var reference = forms["reference"].ToString();

            var userManager = (UserManager<IdentityUser>)services.GetService(typeof(UserManager<IdentityUser>));
            var userIdentity = await userManager.Users.SingleOrDefaultAsync(p => p.PhoneNumber == phoneNumber, cancellationToken);

            var pinService = (IPINService)services.GetService(typeof(IPINService));
            var verifyResult = await pinService.VerifyPINAsync(userIdentity, new KeyValuePair<string, string>(pin, reference), cancellationToken);

            return verifyResult;
        }

        [Authorize]
        [HttpPost]
        [Route("Verify")]
        public async Task<VerifyPINResultModel> Verify(IFormCollection forms, CancellationToken cancellationToken)
        {
            var userManager = (UserManager<IdentityUser>)services.GetService(typeof(UserManager<IdentityUser>));
            var userIdentity = await userManager.FindByNameAsync(User.Identity.Name);

            var pin = forms["pin"].ToString();
            var reference = forms["reference"].ToString();

            var pinService = (IPINService)services.GetService(typeof(IPINService));
            var verifyResult = await pinService.VerifyPINAsync(userIdentity, new KeyValuePair<string, string>(pin, reference), cancellationToken);

            return verifyResult;
        }
    }
}
