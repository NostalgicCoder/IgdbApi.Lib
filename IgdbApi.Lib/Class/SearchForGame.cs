using IgdbApi.Lib.Models;

namespace IgdbApi.Lib.Class
{
    public class SearchForGame
    {
        private Game _gameResult;

        public Game SearchForGameByNameAndPlatform(List<Game> games, string nameOfGame, int platformId = 0)
        {
            _gameResult = new Game();

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

        private Game SearchForGameByNameOnly(List<Game> games, string nameOfGame)
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