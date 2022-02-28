using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Reservation.Models.ConfigModel;
using Reservation.Services.Interfaces;
using System;
using System.Collections.Generic;
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
    }
}
