namespace IgdbApi.Lib.Models
{
    public class FullGameData
    {
        public Game Game { get; set; }
        public List<GameDetails> GameDetails { get; set; }
        public List<Platform> Platforms { get; set; }
        public List<Cover> Covers { get; set; }
        public List<InvolvedCompanies> InvolvedCompanies { get; set; }
        public List<Artworks> Artworks { get; set; }
        public string LargeCoverUrl { get; set; }
        public List<string> LargeArtworkUrls { get; set; }
        public List<Genre> Genres { get; set; } 
    }
}