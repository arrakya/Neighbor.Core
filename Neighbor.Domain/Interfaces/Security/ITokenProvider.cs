using System.Threading.Tasks;

namespace Neighbor.Core.Domain.Interfaces.Security
{
    public interface ITokenProvider
    {
        Task<string> Create(string name, string password);

        Task<bool> Validate(string token);
    }
}
