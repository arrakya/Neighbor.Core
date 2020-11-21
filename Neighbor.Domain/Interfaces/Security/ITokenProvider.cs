﻿using Neighbor.Core.Domain.Models.Identity;
using System.Threading.Tasks;

namespace Neighbor.Core.Domain.Interfaces.Security
{
    public interface ITokenProvider
    {
        Task<string> Create(string name, string password);

        bool Validate(string token);
    }
}
