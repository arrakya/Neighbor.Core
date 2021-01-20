using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neighbor.Server.Identity.Models;
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

        [Authorize]
        [HttpGet]
        [Route("Generate")]
        public async Task<GeneratePINResultModel> Generate(CancellationToken cancellationToken)
        {
            var pinService = (IPINService)services.GetService(typeof(IPINService));
            var pinReference = await pinService.GeneratePINAsync(HttpContext.User.Identity.Name, cancellationToken);

            return pinReference;
        }

        [Authorize]
        [HttpPost]
        [Route("Verify")]
        public async Task<VerifyPINResultModel> Verify(IFormCollection forms, CancellationToken cancellationToken)
        {
            var pin = forms["pin"].ToString();
            var reference = forms["reference"].ToString();

            var pinService = (IPINService)services.GetService(typeof(IPINService));
            var verifyResult = await pinService.VerifyPINAsync(HttpContext.User.Identity.Name, new KeyValuePair<string, string>(pin, reference), cancellationToken);

            return verifyResult;
        }
    }
}
