using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BotenArne.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BotenArne.Models;
using Microsoft.AspNetCore.SignalR;

namespace BotenArne.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<ArneHub> _arneHub;

        public HomeController(ILogger<HomeController> logger, IHubContext<ArneHub> arneHub)
        {
            _logger = logger;
            _arneHub = arneHub;
        }

        public IActionResult Index()
        {
            return View();
        }


        public JsonResult Speak()
        {
            _arneHub.Clients.All.SendAsync("ReceiveCommand", "speak", "En vacker liten text som skall sägas.");
            
            return Json("OK");
        }

        public JsonResult SendAction()
        {
            _arneHub.Clients.All.SendAsync("ReceiveCommand", "Searching");
            
            return Json("OK");
        }

        [HttpPost]
        public JsonResult InputSpeech([FromBody] string speech)
        {
            if (speech.ToLower().Contains("leta"))
            {
                _arneHub.Clients.All.SendAsync("ReceiveCommand", "Searching");
            }

            return Json("OK");
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
