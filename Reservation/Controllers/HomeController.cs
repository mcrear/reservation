using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reservation.Models;
using Reservation.Models.ResponseModel;
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
        private readonly IJourneyService _journeyService;
        private readonly IHttpContextAccessor _accessor;

        public HomeController(ILogger<HomeController> logger, ISessionService sessionService, ILocationService locationService, IJourneyService journeyService, IHttpContextAccessor accessor)
        {
            _logger = logger;
            _sessionService = sessionService;
            _locaciontService = locationService;
            _journeyService = journeyService;
            _accessor = accessor;
        }

        public async Task<IActionResult> IndexAsync()
        {
            
            //var deviceSession = await _sessionService.GetDeviceSession();
            //BusLocationsResponseModel locations = await _locaciontService.GetLocations(new Models.RequestModel.BusLocationsRequestModel
            //{
            //    DeviceSession = deviceSession,
            //    Data = null
            //});

            //BusJourneysResponseModel journeys = await _journeyService.GetBusJourneys(new Models.RequestModel.BusJourneysRequestModel
            //{
            //    Date = DateTime.UtcNow.AddDays(2).ToString("yyyy-MM-dd"),
            //    DeviceSession = new Models.RequestModel.DeviceSession
            //    {
            //        DeviceId = session.Data.DeviceId,
            //        SessionId = session.Data.SessionId,
            //    },
            //    Language = language,
            //    Data = new Models.RequestModel.BusJourneyRequest
            //    {
            //        OriginId = 349, // locations.Data.FirstOrDefault().Id,
            //        DestinationId = 356,// locations.Data.LastOrDefault().Id,
            //        DepartureDate = DateTime.UtcNow.AddDays(2).ToString("yyyy-MM-dd")
            //    }
            //});

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
