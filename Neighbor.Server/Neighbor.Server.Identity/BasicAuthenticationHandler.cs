using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Neighbor.Server.Identity
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var hasAuthenticationHeader = Request.Headers.TryGetValue("Authorization", out var headerAuthentication);
            if (!hasAuthenticationHeader)
            {
                var failResult = await Task.FromResult(AuthenticateResult.Fail("No Authorization in request header"));
                return failResult;
            }

            try
            {
                var credentialBytes = Convert.FromBase64String(headerAuthentication.ToString().Replace("Basic ", string.Empty));
                var credential = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                var clientId = credential[0];
                var clientSecret = credential[1];

                if (clientId != "neighbor_grooveville" || clientSecret != "3100601614660")
                {
                    var failResult = await Task.FromResult(AuthenticateResult.Fail("client_id or client_secret incorrect"));
                    return failResult;
                }

                var claims = new Claim[]
                {
                new Claim("client_id", clientId),
                new Claim("client_secret",clientSecret)
                };
                var claimIdentity = new ClaimsIdentity(claims);
                var claimPrinciple = new ClaimsPrincipal(claimIdentity);
                var result = await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimPrinciple, "Basic")));

                return result;
            }
            catch (Exception ex)
            {
                var failResult = await Task.FromResult(AuthenticateResult.Fail(new Exception("Basic authorization extract error", ex)));
                return failResult;
            }
        }
    }
}
