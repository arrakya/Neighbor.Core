using System.Net.Http;

namespace Neighbor.Mobile.Services.Net
{
    public class CreateHttpClientResult
    {
        private HttpClient httpClient;

        public bool IsReady
        {
            get => httpClient != null;
        }

        public HttpClient HttpClient
        {
            get => httpClient;
            set
            {
                httpClient = value;
            }
        }

        public CreateHttpClientResult()
        {
        }
    }
}
