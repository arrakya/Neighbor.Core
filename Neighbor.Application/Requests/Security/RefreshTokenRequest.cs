using MediatR;
using Neighbor.Core.Application.Responses.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neighbor.Core.Application.Requests.Security
{
    public class RefreshTokenRequest : IRequest<RefreshTokenResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
