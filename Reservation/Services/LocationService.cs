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
        public LocationService(IHttpClientFactory httpClientFactory, IOptions<ApiConfig> config, ISessionService sessionService)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public async Task<BusLocationsResponseModel> GetLocations(BusLocationsRequestModel requestModel)
        {
            var language = CultureInfo.CurrentCulture.IetfLanguageTag;
            requestModel.Date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");
            requestModel.Language = language;

            return await HttpPostHelper<BusLocationsResponseModel>.PostDataAsync(_httpClientFactory, _config, "location/getbuslocations", requestModel);
        }
    }
}
