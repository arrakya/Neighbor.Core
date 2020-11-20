using Neighbor.Core.Domain.Models.Identity;

namespace Neighbor.Core.Application.Responses.Identity
{
    public class GetUserIdentityResponse
    {
        public IdentityUserContext Content { get; set; }
    }
}
