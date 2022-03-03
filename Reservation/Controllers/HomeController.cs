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

        public HomeController(ILogger<HomeController> logger, ISessionService sessionService, ILocationService locationService, IDataProtectionProvider provider)
        {
            _logger = logger;
            _sessionService = sessionService;
            _locaciontService = locationService;
            _protector = provider.CreateProtector("test");
        }

        public async Task<IActionResult> IndexAsync()
        {

            var model = new JourneySearchModel();

            var existingSearch = HttpContext.Request.Cookies["search"];
            if (existingSearch != null)
            {
                var clearText = _protector.Unprotect(existingSearch);
                model = JsonConvert.DeserializeObject<JourneySearchModel>(clearText);
            }

            return View(model);
        }

        [HttpPost]
        [Route("/home/getlocations")]
        public async Task<IActionResult> GetLocations()
        {
            var deviceSession = await _sessionService.GetDeviceSession();

            BusLocationsResponseModel locations = await _locaciontService.GetLocations(new Models.RequestModel.BusLocationsRequestModel
            {
                DeviceSession = deviceSession,
                Data = null
            });

            var locationList = locations.Data.Select(
              x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }
              ).ToList();

            return new JsonResult(locationList);
        }



        [HttpPost]
        [Route("/home/searchlocations")]
        public async Task<IActionResult> SearchLocations(string search, string type, string selected)
        {
            var deviceSession = await _sessionService.GetDeviceSession();

            BusLocationsResponseModel locations = await _locaciontService.GetLocations(new Models.RequestModel.BusLocationsRequestModel
            {
                DeviceSession = deviceSession,
                Data = search
            });

            var locationList = locations.Data.Select(
              x => new { text = x.Name, id = x.Id.ToString() }
              ).ToList();

            return new JsonResult(locationList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
