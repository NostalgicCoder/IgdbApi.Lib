namespace IgdbApi.Lib.Models
{
    public class Platform
    {
        public int id { get; set; }
        public string abbreviation { get; set; }
        public int category { get; set; }
        public int created_at { get; set; }
        public int generation { get; set; }
        public string name { get; set; }
        public int platform_logo { get; set; }
        public string slug { get; set; }
        public int updated_at { get; set; }
        public string url { get; set; }
        public int[] versions { get; set; }
        public string checksum { get; set; }
        public string alternative_name { get; set; }
        public int[] websites { get; set; }
        public int platform_family { get; set; }
        public string summary { get; set; }
    }
}