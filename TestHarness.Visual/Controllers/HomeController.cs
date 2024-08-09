using Microsoft.AspNetCore.Mvc;
using IgdbApi.Lib;
using IgdbApi.Lib.Models;

namespace TestHarness.Visual.Controllers
{
    public class HomeController : Controller
    {
        private Igdb _igdb = new Igdb();

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _igdb.GetTwitchAccessToken();

            FullGameData fullGameData = _igdb.GetAllDataOnAGame("doom");

            if(fullGameData.Game != null)
            {
                return View(fullGameData);
            }

            return View("NoResult");
        }
    }
}