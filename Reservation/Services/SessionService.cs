using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Reservation.Helpers;
using Reservation.Models.ConfigModel;
using Reservation.Models.RequestModel;
using Reservation.Models.ResponseModel;
using Reservation.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Reservation.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<ApiConfig> _config;
        public SessionService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IOptions<ApiConfig> config)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }

        public async Task<SessionResponse> GetSessionAsync(SessionRequestModel requestModel)
        {
            return await HttpPostHelper<SessionResponse>.PostDataAsync(_httpContextAccessor, _httpClientFactory, _config, "/client/getsession", requestModel);
        }
    }
}
