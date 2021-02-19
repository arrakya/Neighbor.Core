using Neighbor.Core.Domain.Models.Security;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Neighbor.Mobile.Services
{
    public class PINService
    {
        public delegate void RequestPINErrorHandler(string errorMessage);
        public event RequestPINErrorHandler OnRequestError;

        public async Task<string> RequestAsync(string phoneNumber, HttpClient httpClient, CancellationToken? cancellationToken = null)
        {
            var response = await httpClient.GetAsync($"pin/generate/{phoneNumber}", cancellationToken.GetValueOrDefault());
            var responseString = await response.Content.ReadAsStringAsync();
            var jsonSerializerOption = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var requestPINResult = JsonSerializer.Deserialize<GeneratePINResultModel>(responseString, jsonSerializerOption);

            if (!requestPINResult.IsSuccess)
            {
                var errorMessage = requestPINResult.Message;
                switch (requestPINResult.Code)
                {
                    case "PIN0002":
                        errorMessage = "Phone number not found";
                        break;
                    case "PIN0003":
                        errorMessage = "Too many time request. Please try again in next 24 hrs.";
                        break;
                }

                OnRequestError?.Invoke(errorMessage);

                return string.Empty;
            }

            return requestPINResult.Reference;
        }
    }
}
