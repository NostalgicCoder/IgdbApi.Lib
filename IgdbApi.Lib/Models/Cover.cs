namespace IgdbApi.Lib.Models
{
    public class Cover
    {
        public int id { get; set; }
        public bool alpha_channel { get; set; }
        public bool animated { get; set; }
        public int game { get; set; }
        public int height { get; set; }
        public string image_id { get; set; }
        public string url { get; set; }
        public int width { get; set; }
        public string checksum { get; set; }
    }
}