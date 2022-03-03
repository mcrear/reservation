using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Reservation.Models;
using Reservation.Models.ResponseModel;
using Reservation.Models.ViewModel;
using Reservation.Services.Interfaces;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISessionService _sessionService;
        private readonly ILocationService _locaciontService;
        private readonly IDataProtector _protector;

        /// <summary>
        /// HomeController sınıfı ayağa kalkarken yapılacak işlemler bu methodda yer almaktadır. Dependecy ile bağladığımız interfacelerin sahip olduğu nesneler private readonly sınıflara set edilerek kapsülasyon yapısı sağlanmış oluyor.
        /// </summary>
        /// <param name="logger">loglama işlemleri için kulllanılacak servis</param>
        /// <param name="sessionService">oturum işlemlerinin yönetilmesi için kullanılacak servis</param>
        /// <param name="locationService">kalkış ve varış noktalarının yönetilmesi için kullanılacak servis</param>
        /// <param name="provider">Cookie işlemlerinde saklanacak olan dataların güvenli olarak saklanması için kullanılacak servis</param>
        public HomeController(ILogger<HomeController> logger, ISessionService sessionService, ILocationService locationService, IDataProtectionProvider provider)
        {
            _logger = logger;
            _sessionService = sessionService;
            _locaciontService = locationService;
            _protector = provider.CreateProtector("test");
        }


        /// <summary>
        /// Controller action belirtilmeden çağırıldığında ayağa kalkacak olan methoddur. Bu method proje içerisinde lokasyonların listelenip seçiminin yapıldığı sayfadır. 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> IndexAsync()
        {
            // View içerisine gönderilecek olan modelin oluşturulması.
            var model = new JourneySearchModel();

            // Cookie içerisinden belirtilen kullanıcının daha önce bir sorgulama yapıp yapmadığının kontrol edilmesi. Eğer search keyine sahip bir cookie var ise daha önce tanımladığımız model içerisine bu cookie değeri setlenecek
            var existingSearch = HttpContext.Request.Cookies["search"];

            // searh key ile çağırdığımız cookienin boş yada dolu olduğunun kontrol edilmesi
            if (existingSearch != null)
            {
                // eğer daha önce sorgu yapılmış ve cookie değeri dolu gelmiş işe bu cookie içerisindeki value değeri daha önce şifrelenerek saklanmış bir değer olacaktır. Bu değeri proje içerisinde kullanabilmek için öncelikle açık metin haline dönüştürmemiz gerekir.
                var clearText = _protector.Unprotect(existingSearch);

                // almış olduğumuz açık metin daha öcne yapılan sorgumuzun modelinin json formatına dönüştürülmüş hali olacaktır. Bu yüzden modelimizi tekrar oluşturabilmek için daha önce oluşturduğumuz verimizi deserialize ederek modelimizi dolduruyoruz.
                model = JsonConvert.DeserializeObject<JourneySearchModel>(clearText);

                // ileride karşılaşabileceğimiz olası sorunlar için yapılan sorgu değerinide log olarak saklıyoruz.
                _logger.LogInformation("Values read from cookie ", model);
            }

            return View(model);
        }


        /// <summary>
        /// Home controller sayfası ilk açıldığında boş olarak gelen select optionsların dataları buradan doldurulmaktadır. hem source hem de destination location listesi buraya sorgu atmaktadır. burada herhangi bir listeleme yada filtreleme yapılmaksızın tüm lokasyonlar listelenir.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("/home/getlocations")]
        public async Task<IActionResult> GetLocations()
        {
            // Kullanılan cihaza ait oturum bilgilerinin okunması işlemi
            var deviceSession = await _sessionService.GetDeviceSession();

            // ileride karşılaşılabileceğimiz olası sorunlar için cihaz bilgilerinin log olarak saklanması
            _logger.LogInformation("Current device session ", deviceSession);

            // servis üzerinden lokasyon listesinin alınması
            BusLocationsResponseModel locations = await _locaciontService.GetLocations(new Models.RequestModel.BusLocationsRequestModel
            {
                DeviceSession = deviceSession,
                Data = null
            });

            // lokasyon listeleri ön yüzde select2 içersinde kullanılmaktadır. bu yüzden okunan veriler beklenen formata çevrilerek ön yüze iletilmektedir.
            var locationList = locations.Data.Select(
              x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }
              ).ToList();

            // yapılan sorgu sonucu herhangi bir verinin dönmemesi durumu göz önüne alınıp bir uyarı logu oluşturulur.
            if (locationList.Count == 0)
                _logger.LogWarning("No location listed ", locationList);

            // oluşturduğumuz ve select2 nesnesinin datasource kısmı ile birebir uyumlu olan listemizin json olarak geri döndürülmesi
            return new JsonResult(locationList);
        }


        /// <summary>
        ///  Bu method ile lokasyon arama işlemleri yönetilmektedir. Select2 nesnesinden gelen anahtar kelime lokasyon servisi içerisine gönderilip, dönen sonuca göre nesne doldurulmaktadır
        /// </summary>
        /// <param name="search">Arama anahtar kelimemizin geldiği parametre</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/home/searchlocations")]
        public async Task<IActionResult> SearchLocations(string search)
        {
            // Kullanılan cihaza ait oturum bilgilerinin okunması işlemi
            var deviceSession = await _sessionService.GetDeviceSession();

            // servis üzerinden anahtar kelimeye göre filtrelenmiş lokasyon listesinin alınması
            BusLocationsResponseModel locations = await _locaciontService.GetLocations(new Models.RequestModel.BusLocationsRequestModel
            {
                DeviceSession = deviceSession,
                Data = search
            });

            // lokasyon listeleri ön yüzde select2 içersinde kullanılmaktadır. bu yüzden okunan veriler beklenen formata çevrilerek ön yüze iletilmektedir.
            var locationList = locations.Data.Select(
              x => new { text = x.Name, id = x.Id.ToString() }
              ).ToList();

            // yapılan sorgu sonucu herhangi bir verinin dönmemesi durumu göz önüne alınıp bir uyarı logu oluşturulur.
            if (locationList.Count == 0)
                _logger.LogWarning("No location listed. Search value ", search);

            return new JsonResult(locationList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
