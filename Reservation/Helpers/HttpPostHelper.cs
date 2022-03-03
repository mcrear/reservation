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
    /// <summary>
    /// Generic şekilde yönetilebilen post helper sınıfıdır. Alınmak istenen sonuç parametre olarak geçilmelidir.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public static class HttpPostHelper<TResponse>
    {
        /// <summary>
        /// Post işlemleri bu method üzerinden yapılır. Gönderilmiş response tipini geri dönmesi beklenir.
        /// </summary>
        /// <param name="clientFactory">Post işleminin yapılması için ihtiyaç duyulan client nesnesinin oluşturulacağı factoryi taşır.</param>
        /// <param name="config">Api ile ilgili değişken değerlerin okunabilmesi için gerekli konfigurasyon ayarlarının okunduğu servistir.</param>
        /// <param name="path">Base path dışında controller ve action yolunu barındırır. Post edilecek url oluşturulurken baseUrl adresinin sonuna eklenir.</param>
        /// <param name="request">post işleminde gönderilecek olan bilgilerin bulunduğu nesne</param>
        /// <returns></returns>
        public static async Task<TResponse> PostDataAsync(IHttpClientFactory clientFactory, IOptions<ApiConfig> config, string path, object request)
        {
            // yeni bir client nesnesinin oluşturulması.
            using var client = clientFactory.CreateClient();

            // post işleminde karşılaşılabilecek olası hatalar için hata kontrolünün yapılması.
            try
            {
                // app settin dosyası içerisinde yetkilendirme olup olmadığını var ise hangi tür olduğunun kontrolünün yapıldığı yerdir.
                if (config.Value.UseAuthorization)
                {
                    // eğer bir yetkilendirme kullanılmış ise bunun değerin istek parametresinin başlık alanına eklenmesi işlemini yapar.
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(config.Value.AuthType, config.Value.Token);
                }

                // request içerisinde bulunan dataların eklenip client üzerinden post işleminin yapılması 
                var response = await client.PostAsync($"{config.Value.BaseUrl}/{path}",
                  new StringContent(
                      JsonConvert.SerializeObject(request),
                      Encoding.UTF8,
                      "application/json")).ConfigureAwait(false);
                // post işleminden dönen sonucun string formatında dinlenilmesi
                var data = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                // dönen sonuncun generic olarak beklenen tipe cast edilmesi işlemi
                var res = JsonConvert.DeserializeObject<TResponse>(data);

                // sonucun geri döndürülmesi
                return res;

            }
            catch (Exception ex)
            {
                // olası bir erişim sorunu için oluşturulan custom exception bir nesne fırlatılması
                throw new AppException(ex.Message);
            }
        }
    }
}

