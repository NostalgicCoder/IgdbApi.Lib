using IgdbApi.Lib.Models;

namespace IgdbApi.Lib.Interfaces
{
    public interface IFullGameData
    {
        IgdbGame Game { get; set; }
        List<GameDetails> GameDetails { get; set; }
        List<Platform> Platforms { get; set; }
        List<Cover> Covers { get; set; }
        List<InvolvedCompanies> InvolvedCompanies { get; set; }
        List<Artworks> Artworks { get; set; }
        string LargeCoverUrl { get; set; }
        List<string> LargeArtworkUrls { get; set; }
        List<Genre> Genres { get; set; }
    }
}