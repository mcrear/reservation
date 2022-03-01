using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Reservation.Models;
using Reservation.Models.ResponseModel;
using Reservation.Models.ViewModel;
using Reservation.Services.Interfaces;
using Shyjus.BrowserDetection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISessionService _sessionService;
        private readonly ILocationService _locaciontService;

        public HomeController(ILogger<HomeController> logger, ISessionService sessionService, ILocationService locationService)
        {
            _logger = logger;
            _sessionService = sessionService;
            _locaciontService = locationService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            return View(new JourneySearchModel());
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
