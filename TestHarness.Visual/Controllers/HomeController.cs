using Microsoft.AspNetCore.Mvc;
using IgdbApi.Lib;
using IgdbApi.Lib.Interfaces;

namespace TestHarness.Visual.Controllers
{
    public class HomeController : Controller
    {
        private IIgdb _igdb = new Igdb();

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string nameOfGame)
        {
            // Pass over 'clientId' and 'clientSecret' that is unique to the users (Twitch access) account here:
            _igdb.GetTwitchAccessToken("PRIVATE", "PRIVATE");

            IFullGameData fullGameData = _igdb.GetAllDataOnAGame(nameOfGame);

            if(fullGameData.Game != null && fullGameData.Covers != null)
            {
                return View(fullGameData);
            }

            return View("NoResult");
        }
    }
}