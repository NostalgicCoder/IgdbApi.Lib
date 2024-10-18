using IgdbApi.Lib.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TestHarness.Visual.Models
{
    public class GameSearch
    {
        public IFullGameData FullGameData { get; set; }

        public string NameOfGame { get; set; }
        public string SelectedPlatform { get; set; }

        public List<SelectListItem> Platforms = new List<SelectListItem>()
        {
            new SelectListItem {Text = "PC", Value = "6"},
            new SelectListItem {Text = "Amiga", Value = "16"},
            new SelectListItem {Text = "Atari ST", Value = "63"},
            new SelectListItem {Text = "Spectrum", Value = "26"},
            new SelectListItem {Text = "Commodore 64", Value = "15"},
            new SelectListItem {Text = "Amstrad", Value = "25"},
            new SelectListItem {Text = "Xbox 360", Value = "12"},
            new SelectListItem {Text = "Xbox One", Value = "49"},
            new SelectListItem {Text = "Playstation 2", Value = "8"},
            new SelectListItem {Text = "Playstation 3", Value = "9"},
            new SelectListItem {Text = "Playstation 4", Value = "48"},
            new SelectListItem {Text = "Playstation 5", Value = "167"},
        };
    }
}