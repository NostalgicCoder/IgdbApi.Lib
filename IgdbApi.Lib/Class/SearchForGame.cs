using IgdbApi.Lib.Enum;
using IgdbApi.Lib.Models;

namespace IgdbApi.Lib.Class
{
    public class SearchForGame
    {
        private IgdbGame _gameResult;

        public IgdbGame SearchForGameByNameAndPlatform(List<IgdbGame> games, string nameOfGame, int platformId = 0)
        {
            _gameResult = new IgdbGame();

            // INFO: Using this instead of a LAMBDA expression as sometimes the object 'Platforms' can be NULL for a item causing the search to explode and fail, this gets round it. Possible marginal performance penalty however due to it.
            foreach(IgdbGame item in games)
            {
                if (item.platforms != null)
                {
                    // Removes some rogue results
                    if (item.platforms.Contains((int)PlatformEnum.HandHeldLCDGame))
                    {
                        games.Remove(item);
                        break;
                    }

                    if (platformId != 0)
                    {
                        // Search by direct match on name and platform
                        if (item.name.ToLower() == nameOfGame.ToLower() && item.platforms.Contains(platformId))
                        {
                            _gameResult = item;
                            break;
                        }

                        // Search by direct match on platform but partial match on name
                        if (item.name.ToLower().Contains(nameOfGame.ToLower()) && item.platforms.Contains(platformId))
                        {
                            _gameResult = item;
                            break;
                        }
                    }
                }
            }

            if(platformId == 0)
            {
                _gameResult = SearchForGameByNameOnly(games, nameOfGame);
            }

            return _gameResult;
        }

        private IgdbGame SearchForGameByNameOnly(List<IgdbGame> games, string nameOfGame)
        {
            // Search by direct match on name
            _gameResult = games.Where(x => x.name.ToLower() == nameOfGame.ToLower()).FirstOrDefault();

            if (_gameResult == null)
            {
                // Search by partial match on name
                _gameResult = games.Where(x => x.name.ToLower().Contains(nameOfGame.ToLower())).FirstOrDefault();
            }

            if (_gameResult == null && games.Count() != 0)
            {
                // If unable to match on any other criteria then just pick the first result
                _gameResult = games.FirstOrDefault();
            }

            return _gameResult;
        }
    }
}