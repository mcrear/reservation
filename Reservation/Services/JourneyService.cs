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
    public class JourneyService : IJourneyService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<ApiConfig> _config;
        public JourneyService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IOptions<ApiConfig> config)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }

        public async Task<BusJourneysResponseModel> GetBusJourneys(BusJourneysRequestModel requestModel)
        {
            var language = CultureInfo.CurrentCulture.IetfLanguageTag;
            requestModel.Date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");
            requestModel.Language = language;
            return await HttpPostHelper<BusJourneysResponseModel>.PostDataAsync( _httpClientFactory, _config, "journey/getbusjourneys", requestModel);
        }
    }
}
