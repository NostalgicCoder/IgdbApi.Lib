namespace IgdbApi.Lib.Models
{
    public class SearchResult
    {
        public int id { get; set; }
        public string alternative_name { get; set; }
        public int game { get; set; }
        public string name { get; set; }
        public int published_at { get; set; }
        public int collection { get; set; }
        public int character { get; set; }
    }
}