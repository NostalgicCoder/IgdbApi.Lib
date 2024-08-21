using IgdbApi.Lib.Models;

namespace IgdbApi.Lib.Class
{
    public class SearchForGame
    {
        private IgdbGame _gameResult;

        public IgdbGame SearchForGameByNameAndPlatform(List<IgdbGame> games, string nameOfGame, int platformId = 0)
        {
            _gameResult = new IgdbGame();

            if (platformId != 0)
            {
                // Search by direct match on name and platform
                _gameResult = games.Where(x => x.name.ToLower() == nameOfGame.ToLower() && x.platforms.Contains(platformId)).FirstOrDefault();

                if(_gameResult == null)
                {
                    // Search by direct match on platform but partial match on name
                    _gameResult = games.Where(x => x.name.ToLower().Contains(nameOfGame.ToLower()) && x.platforms.Contains(platformId)).FirstOrDefault();
                }

                if (_gameResult == null)
                {
                    _gameResult = SearchForGameByNameOnly(games, nameOfGame);
                }
            }
            else
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

            return _gameResult;
        }
    }
}