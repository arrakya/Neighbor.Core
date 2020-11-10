using System;

namespace Neighbor.Core.Domain.Interfaces.Security
{
    public interface ITokenProvider
    {
        string Create(double tokenLifeTimeInSec);

        bool Validate(string token);
    }
}
