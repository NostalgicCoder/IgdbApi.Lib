using IgdbApi.Lib;

namespace TestHarness.Console
{
    public class Run
    {
        public void CallApi()
        {
            Igdb igdb = new Igdb();

            igdb.GetTwitchAccessToken();
            igdb.GetAllDataOnAGame("fallout: new vegas");

            System.Console.ReadLine();
        }
    }
}