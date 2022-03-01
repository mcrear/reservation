using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public JourneyController(ILogger<JourneyController> logger, ISessionService sessionService, IJourneyService journeyService)
        {
            _logger = logger;
            _sessionService = sessionService;
            _journeyService = journeyService;
        }
        [HttpPost]
        public async Task<IActionResult> IndexAsync(JourneySearchModel model)
        {
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
