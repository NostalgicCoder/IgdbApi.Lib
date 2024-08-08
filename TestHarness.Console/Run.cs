using IgdbApi.Lib;

namespace TestHarness.Console
{
    public class Run
    {
        public void CallApi()
        {
            Igdb igdb = new Igdb();

            igdb.GetTwitchAccessToken();
            //igdb.GetCoverArtUrl("Fallout: New Vegas");

            igdb.GetPlatforms();

            System.Console.ReadLine();
        }
    }
}