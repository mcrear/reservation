using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Reservation.Models.ConfigModel;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Helpers
{
    public static class HttpPostHelper<TResponse>
    {
        public static async Task<TResponse> PostDataAsync(IHttpClientFactory clientFactory, IOptions<ApiConfig> config, string path, object request)
        {
            using var client = clientFactory.CreateClient();
            try
            {
                if (config.Value.UseAuthorization)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(config.Value.AuthType, config.Value.Token);
                }

                var response = await client.PostAsync($"{config.Value.BaseUrl}/{path}",
                  new StringContent(
                      JsonConvert.SerializeObject(request),
                      Encoding.UTF8,
                      "application/json")).ConfigureAwait(false);
                var data = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                var res = JsonConvert.DeserializeObject<TResponse>(data);
                return res;

            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
        }
    }
}
