namespace IgdbApi.Lib.Models
{
    public class FullGameData
    {
        public Game Game { get; set; }
        public List<GameDetails> GameDetails { get; set; }
        public List<Platform> Platforms { get; set; }
        public List<Cover> Covers { get; set; }
        public List<InvolvedCompanies> InvolvedCompanies { get; set; }
    }
}