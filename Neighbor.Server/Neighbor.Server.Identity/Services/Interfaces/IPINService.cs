using Neighbor.Server.Identity.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbor.Server.Identity.Services.Interfaces
{
    public interface IPINService
    {
        Task<GeneratePINResultModel> GeneratePINAsync(string userName, CancellationToken cancellationToken);
        Task<VerifyPINResultModel> VerifyPINAsync(string userName, KeyValuePair<string, string> pinAndRef, CancellationToken cancellationToken);
    }
}
