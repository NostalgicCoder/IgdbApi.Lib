using Microsoft.AspNetCore.Mvc;
using IgdbApi.Lib;
using IgdbApi.Lib.Interfaces;
using TestHarness.Visual.Models;
using IgdbApi.Lib.Models;

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

        public IActionResult Index(GameSearch gameSearch)
        {
            // Pass over 'clientId' and 'clientSecret' that is unique to the users (Twitch access) account here:
            _igdb.GetTwitchAccessToken("PRIVATE", "PRIVATE");

            if (!string.IsNullOrEmpty(gameSearch.NameOfGame) && !string.IsNullOrEmpty(gameSearch.SelectedPlatform) && gameSearch.SelectedPlatform != "Please select one")
            {
                gameSearch.FullGameData = _igdb.GetAllDataOnAGame(gameSearch.NameOfGame, Int32.Parse(gameSearch.SelectedPlatform));
            }

            if (gameSearch.FullGameData == null)
            {
                gameSearch.FullGameData = new FullGameData();
            }

            return View(gameSearch);
        }
    }
}