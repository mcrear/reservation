using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Reservation.Helpers;
using Reservation.Models.ConfigModel;
using Reservation.Models.Dto;
using Reservation.Models.RequestModel;
using Reservation.Models.ResponseModel;
using Reservation.Services.Interfaces;
using Shyjus.BrowserDetection;
using System.Net.Http;
using System.Threading.Tasks;

namespace Reservation.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<ApiConfig> _config;
        private readonly IDataProtector _protector;
        private readonly IBrowserDetector _browserDetector;
        public SessionService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IOptions<ApiConfig> config, IDataProtectionProvider provider, IBrowserDetector browserDetector)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _config = config;
            _protector = provider.CreateProtector(GetType().FullName);
            _browserDetector = browserDetector;
        }

        public async Task<DeviceSession> GetDeviceSession()
        {

            var DeviceId = _httpContextAccessor.HttpContext.Request.Cookies["DeviceId"];
            var SessionId = _httpContextAccessor.HttpContext.Request.Cookies["SessionId"];

            if (DeviceId == null || SessionId == null)
            {
                SessionResponse response = new SessionResponse();

                var browser = _browserDetector.Browser;
                var ClientIPAddr = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString() == "::1" ? "165.114.41.21" : _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
                var port = _httpContextAccessor.HttpContext.Connection.RemotePort.ToString();

                SessionRequestModel requestModel = new SessionRequestModel
                {
                    Browser = new Browser
                    {
                        Name = browser.Name,
                        Version = browser.Version
                    },
                    Connection = new Connection
                    {
                        IpAddress = ClientIPAddr,
                        Port = port
                    },
                    Type = 1
                };

                response = await HttpPostHelper<SessionResponse>.PostDataAsync(_httpClientFactory, _config, "client/getsession", requestModel);


                if (_httpContextAccessor.HttpContext != null)
                {
                    var encryptedDeviceId = _protector.Protect(response.Data.DeviceId.ToString());
                    _httpContextAccessor.HttpContext.Response.Cookies.Append("DeviceId", encryptedDeviceId);

                    var encryptedSessionId = _protector.Protect(response.Data.SessionId.ToString());
                    _httpContextAccessor.HttpContext.Response.Cookies.Append("SessionId", encryptedSessionId);
                }

                return new DeviceSession
                {

                    DeviceId = response.Data.DeviceId.ToString(),
                    SessionId = response.Data.SessionId.ToString()
                };

            }

            return new DeviceSession
            {

                DeviceId = _protector.Unprotect(_httpContextAccessor.HttpContext.Request.Cookies["DeviceId"]),
                SessionId = _protector.Unprotect(_httpContextAccessor.HttpContext.Request.Cookies["SessionId"])
            };
        }
    }
}
