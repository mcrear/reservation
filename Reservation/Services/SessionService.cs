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

        /// <summary>
        /// Oturum bilgilerinin yönetildiği servistir.
        /// </summary>
        /// <param name="httpClientFactory">istek oluşturmak için yeni bir client türetmek üzere hazırlanan client factory</param>
        /// <param name="httpContextAccessor">Servis katmanında HttpContext nesnesine erişebilmek için kullanacağımız servis</param>
        /// <param name="config">yapılandırma ayarlarını kullanmak için oluşturulan servis</param>
        /// <param name="provider">cookies içerisinde saklanacak olan dataların encrypt edilmesi için kullanacağımız servis</param>
        /// <param name="browserDetector">kullanılan browser bilgilierinin çekileceği servis</param>
        public SessionService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IOptions<ApiConfig> config, IDataProtectionProvider provider, IBrowserDetector browserDetector)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _config = config;
            _protector = provider.CreateProtector(GetType().FullName);
            _browserDetector = browserDetector;
        }

        /// <summary>
        /// Cihaza ait oturum bilgilerinin çekileceği method
        /// </summary>
        /// <returns></returns>
        public async Task<DeviceSession> GetDeviceSession()
        {
            try
            {
                // request cookie içerisinde Device ve Session Id bilgilerinin okunması
                var DeviceId = _httpContextAccessor.HttpContext.Request.Cookies["DeviceId"];
                var SessionId = _httpContextAccessor.HttpContext.Request.Cookies["SessionId"];

                // her iki değerinde dolu gelip gelmediğinin kontrol edilmesi
                if (DeviceId == null || SessionId == null)
                {

                    // eğer boş gelen değer var ise cihaz bilgilerine ait cihaz oturumu yeniden oluşturulur.
                    SessionResponse response = new SessionResponse();

                    // kullanılan browser bilgilerinin okunması
                    var browser = _browserDetector.Browser;

                    // local host üzerinden çalışırken dönen ::1 değeri için geçersiz hatası alındığı için localde çalışırken postman collectionda gönderilen değerin set edilmesi
                    var ClientIPAddr = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString() == "::1" ? "165.114.41.21" : _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();

                    // port numarasının okunması
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

                    // session bilgileri için apiye istek atılması
                    response = await HttpPostHelper<SessionResponse>.PostDataAsync(_httpClientFactory, _config, "client/getsession", requestModel);

                    // eğer context boş değil ise response değerine şifrelenmiş oturum ve cihaz bilgileri cookie olarak ekleniyor.
                    if (_httpContextAccessor.HttpContext != null)
                    {
                        var encryptedDeviceId = _protector.Protect(response.Data.DeviceId.ToString());
                        _httpContextAccessor.HttpContext.Response.Cookies.Append("DeviceId", encryptedDeviceId);

                        var encryptedSessionId = _protector.Protect(response.Data.SessionId.ToString());
                        _httpContextAccessor.HttpContext.Response.Cookies.Append("SessionId", encryptedSessionId);
                    }

                    // cihaz bilgilerinin bir üst katmana dönülmesi
                    return new DeviceSession
                    {

                        DeviceId = response.Data.DeviceId.ToString(),
                        SessionId = response.Data.SessionId.ToString()
                    };

                }


                // eğer hem session hemde device bilgileri cookie içerisinde saklanıyorsa bu değerler clear hale getirilip bir üst katmana geri döndürülüyor
                return new DeviceSession
                {

                    DeviceId = _protector.Unprotect(_httpContextAccessor.HttpContext.Request.Cookies["DeviceId"]),
                    SessionId = _protector.Unprotect(_httpContextAccessor.HttpContext.Request.Cookies["SessionId"])
                };
            }
            catch (System.Exception e)
            {
                throw new AppException(e.Message);
            }
        }
    }
}
