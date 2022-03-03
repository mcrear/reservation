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
        public JourneyController(ILogger<JourneyController> logger, ISessionService sessionService, IJourneyService journeyService, IDataProtectionProvider provider)
        {
            _logger = logger;
            _sessionService = sessionService;
            _journeyService = journeyService;
            _protector = provider.CreateProtector("test");
        }
        [HttpPost]
        public async Task<IActionResult> IndexAsync(JourneySearchModel model)
        {

            model.SetSourceId = model.SourceId;
            model.SetDestinationId = model.DestinationId;

            var encryptText = _protector.Protect(JsonConvert.SerializeObject(model));
            HttpContext.Response.Cookies.Append("search", encryptText);

            var deviceSession = await _sessionService.GetDeviceSession();

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

            foreach (var item in journeys.Data.ToList())
            {
                model.Journeys.Add(item);
            }

            return View(model);
        }
    }
}
