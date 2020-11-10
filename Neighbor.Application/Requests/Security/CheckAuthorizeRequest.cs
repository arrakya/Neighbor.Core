using MediatR;
using Neighbor.Core.Application.Responses.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neighbor.Core.Application.Requests.Security
{
    public class CheckAuthorizeRequest : IRequest<CheckAuthorizeResponse>
    {
        public string Token { get; set; }
    }
}
