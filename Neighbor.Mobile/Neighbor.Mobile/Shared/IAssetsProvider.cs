using System.Threading.Tasks;

namespace Neighbor.Mobile.Shared
{
    public interface IAssetsProvider
    {
        Task<T> Get<T>(string name);
    }
}
