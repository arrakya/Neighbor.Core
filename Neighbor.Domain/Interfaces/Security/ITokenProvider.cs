using System.Threading.Tasks;

namespace Neighbor.Core.Domain.Interfaces.Security
{
    public interface ITokenProvider
    {
        Task<string> Create(double tokenLifeTimeInSec);

        bool Validate(string token);
    }
}
