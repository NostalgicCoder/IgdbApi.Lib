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
            igdb.GetAllDataOnAGame("Silent Hill 2", (int)PlatformEnum.PS5);

            System.Console.ReadLine();
        }
    }
}