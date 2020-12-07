using Neighbor.Mobile.Droid;
using Neighbor.Mobile.Shared;
using System;
using System.IO;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(AssetsProvider))]
namespace Neighbor.Mobile.Droid
{
    public class AssetsProvider : IAssetsProvider
    {
        public async Task<T> Get<T>(string name)
        {
            var cert = MainActivity.AssetManager.Open("arrakya.thddns.net.crt");
            using (var sr = new StreamReader(cert))
            {
                var mem = new MemoryStream();
                await sr.BaseStream.CopyToAsync(mem);
                var buff = mem.GetBuffer();

                var result = (T)Convert.ChangeType(buff, typeof(T));

                return result;
            }
        }
    }
}