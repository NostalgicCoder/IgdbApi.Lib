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

            // Removes some rogue results
            games.RemoveAll(x => x.platforms != null && x.platforms.Contains((int)PlatformEnum.HandHeldLCDGame));

            if(platformId != 0)
            {
                List<IgdbGame> fullMatch = games.Where(x => x.name.ToLower() == nameOfGame.ToLower() && x.platforms != null && x.platforms.Contains(platformId)).ToList();
                List<IgdbGame> partialMatch = games.Where(x => x.name.ToLower().Contains(nameOfGame.ToLower()) && x.platforms != null && x.platforms.Contains(platformId)).ToList();

                if(fullMatch.Count != 0)
                {
                    _gameResult = fullMatch.FirstOrDefault();
                }
                else
                {
                    if(partialMatch.Count != 0)
                    {
                        // You may get more then one result here, but we will take the first:
                        _gameResult = partialMatch.FirstOrDefault();
                    }
                    else
                    {
                        _gameResult = SearchForGameByNameOnly(games, nameOfGame);
                    }
                }
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