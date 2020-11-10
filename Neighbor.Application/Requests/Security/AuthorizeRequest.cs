using MediatR;
using Neighbor.Core.Application.Responses.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neighbor.Core.Application.Requests.Security
{
    public class AuthorizeRequest : IRequest<AuthorizeResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
