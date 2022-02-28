using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Reservation.Models;
using Reservation.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISessionService _sessionService;

        public HomeController(ILogger<HomeController> logger, ISessionService sessionService)
        {
            _logger = logger;
            _sessionService = sessionService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var test = await _sessionService.GetSessionAsync(new Models.RequestModel.SessionRequestModel
            {
                Browser = new Models.RequestModel.Browser
                {
                    Name = "Chrome",
                    Version = "47.0.0.12"
                },
                Connection = new Models.RequestModel.Connection
                {
                    IpAddress = "165.114.41.21",
                    Port = "5117"
                },
                Type = 1
            });
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
