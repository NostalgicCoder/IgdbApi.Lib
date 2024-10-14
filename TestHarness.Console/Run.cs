using IgdbApi.Lib;
using IgdbApi.Lib.Enum;
using IgdbApi.Lib.Interfaces;

namespace TestHarness.Console
{
    public class Run
    {
        public void CallApi()
        {
            IIgdb igdb = new Igdb();

            // Pass over 'clientId' and 'clientSecret' that is unique to the users (Twitch access) account here:
            igdb.GetTwitchAccessToken("PRIVATE", "PRIVATE");
            igdb.GetAllDataOnAGame("rainbow islands", (int)PlatformEnum.Amiga);

            System.Console.ReadLine();
        }
    }
}