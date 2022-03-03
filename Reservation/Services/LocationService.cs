using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Reservation.Helpers;
using Reservation.Models.ConfigModel;
using Reservation.Models.RequestModel;
using Reservation.Models.ResponseModel;
using Reservation.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Reservation.Services
{
    public class LocationService : ILocationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<ApiConfig> _config;

        /// <summary>
        ///  lokasyon işlemlerinin yönetilmesi için kullanılacak olan servistir.
        /// </summary>
        /// <param name="httpClientFactory">istek oluşturmak için yeni bir client türetmek üzere hazırlanan client factory</param>
        /// <param name="config">yapılandırma ayarlarını kullanmak için oluşturulan servis</param>
        public LocationService(IHttpClientFactory httpClientFactory, IOptions<ApiConfig> config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        /// <summary>
        /// Lokasyon sorgusu için gelen modele ait requestin tamamlanıp helper üzerinden post edildiği methoddur. Sonuç olarak BusLocationsResponseModel döner
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public async Task<BusLocationsResponseModel> GetLocations(BusLocationsRequestModel requestModel)
        {
            // kullanılan sistemin dil bilgisinin çekilmesi
            var language = CultureInfo.CurrentCulture.IetfLanguageTag;

            // istek zamanının alınmasın
            requestModel.Date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");

            // sistem dilinin istek dili olarak setlenmesi
            requestModel.Language = language;

            // isteğin helper üzerinden post işleminin alınıp dönen sonucun bir önceki katmana dönülmesi
            return await HttpPostHelper<BusLocationsResponseModel>.PostDataAsync(_httpClientFactory, _config, "location/getbuslocations", requestModel);
        }
    }
}
