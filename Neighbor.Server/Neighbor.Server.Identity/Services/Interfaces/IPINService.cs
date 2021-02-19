using Microsoft.AspNetCore.Identity;
using Neighbor.Core.Domain.Models.Security;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbor.Server.Identity.Services.Interfaces
{
    public interface IPINService
    {
        Task<GeneratePINResultModel> GeneratePINAsync(IdentityUser identityUser, CancellationToken cancellationToken);
        Task<VerifyPINResultModel> VerifyPINAsync(IdentityUser identityUser, KeyValuePair<string, string> pinAndRef, CancellationToken cancellationToken);
    }
}
