using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Reservation.Models.Dto;
using Reservation.Models.RequestModel;
using Reservation.Models.ResponseModel;
using Reservation.Models.ViewModel;
using Reservation.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Controllers
{
    public class JourneyController : Controller
    {
        private readonly ILogger<JourneyController> _logger;
        private readonly ISessionService _sessionService;
        private readonly IJourneyService _journeyService;
        private readonly IDataProtector _protector;

        /// <summary>
        /// JourneyController içerisinde kullanılacak olan servislerin nesnelerinin private nesneler içerisine atılması
        /// </summary>
        /// <param name="logger">loglama işlemleri için kulllanılacak servis</param>
        /// <param name="sessionService">oturum işlemlerinin yönetilmesi için kullanılacak servis</param>
        /// <param name="journeyService">seferlerin yönetilmesi için kullanılacak servis</param>
        /// <param name="provider">Cookie işlemlerinde saklanacak olan dataların güvenli olarak saklanması için kullanılacak servis</param>
        public JourneyController(ILogger<JourneyController> logger, ISessionService sessionService, IJourneyService journeyService, IDataProtectionProvider provider)
        {
            _logger = logger;
            _sessionService = sessionService;
            _journeyService = journeyService;
            _protector = provider.CreateProtector("test");
        }

        /// <summary>
        /// HomeController içerisinde oluşturulan ve yönlendirilen searchModel propertylerine göre sorgulama işlemi yapılıp sonucun dönüldüğü method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> IndexAsync(JourneySearchModel model)
        {
            // bir sonraki sorgu işlemi için saklanmak üzere temp dataların oluşturulması.
            // bu işlemler ön yüzde hidden olarak tutulup eğer lokasyonlar doldurulurken seçili değer ile eşleşen lokasyon var ise seçili olarak gelmesi sağlanmaktadır.
            model.SetSourceId = model.SourceId;
            model.SetDestinationId = model.DestinationId;

            // gelen modelin sayfa tekrar açıldığında tekrar okunabilmesi için cookie içerisine saklama işlemi burada yapılmaktadır.
            // model ilk olarak json formatına çevrilip sonrasında encrypted şekilde cookie içerisinde saklanmaktadır.
            var encryptText = _protector.Protect(JsonConvert.SerializeObject(model));
            HttpContext.Response.Cookies.Append("search", encryptText);


            // Kullanılan cihaza ait oturum bilgilerinin okunması işlemi
            var deviceSession = await _sessionService.GetDeviceSession();

            // gelen modele göre sorgu işleminin yapılması
            BusJourneysResponseModel journeys = await _journeyService.GetBusJourneys(new BusJourneysRequestModel
            {
                DeviceSession = deviceSession,
                Data = new BusJourneyRequest
                {
                    OriginId = model.SourceId,
                    DestinationId = model.DestinationId,
                    DepartureDate = model.DateInput
                }
            });

            // servis üzerinden gelen datanın view model içersine aktarılması
            model.Journeys = new List<BusJourney>(journeys.Data.ToList());

            // modelin view içerisine döndürülmesi
            return View(model);
        }
    }
}
