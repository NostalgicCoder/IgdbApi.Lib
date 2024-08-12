using IgdbApi.Lib;
using IgdbApi.Lib.Enum;

namespace TestHarness.Console
{
    public class Run
    {
        public void CallApi()
        {
            Igdb igdb = new Igdb();

            igdb.GetTwitchAccessToken();
            igdb.GetAllDataOnAGame("rainbow islands", (int)PlatformEnum.Amiga);

            System.Console.ReadLine();
        }
    }
}