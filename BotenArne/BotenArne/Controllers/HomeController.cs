using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BotenArne.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BotenArne.Models;
using GemGymmet;
using Microsoft.AspNetCore.SignalR;

namespace BotenArne.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<ArneHub> _arneHub;
        private readonly ArneActionService _predictionService = new ArneActionService();

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
        public IActionResult InputChatCommand([FromBody] string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                return BadRequest();
            }

            _arneHub.Clients.All.SendAsync("ReceiveCommand", command);

            return Ok();

        }

        [HttpPost]
        public IActionResult InputSpeech([FromBody] string speech)
        {

            if (string.IsNullOrEmpty(speech))
            {
                return BadRequest();
            }

            var prediction = _predictionService.PredictAction(speech);
            if (prediction != "NoAction")
            {
                _arneHub.Clients.All.SendAsync("ReceiveCommand", prediction);
            }

            //CheckingSomething - "läsa på"

            //GestureUp "upp"
            //GestureDown "ner"

            //GetArtsy GetTechy -Avancerade Avancerat Komplicerat

            //Processing - "Gräva"

            //Searching "leta"

            //Thinking "fundera" "tänka"

            //Wave - "viktigt"



            //if (speech.ToLower().Contains("leta"))
            //{
            //    _arneHub.Clients.All.SendAsync("ReceiveCommand", "Searching");
            //}
            //else if (speech.ToLower().Contains("gräv"))
            //{
            //    _arneHub.Clients.All.SendAsync("ReceiveCommand", "Processing");
            //}
            //else if (speech.ToLower().Contains("fundera"))
            //{
            //    _arneHub.Clients.All.SendAsync("ReceiveCommand", "Thinking");
            //}

            return Ok();
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
