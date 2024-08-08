using IgdbApi.Lib.Models;
using Newtonsoft.Json;
using RestSharp;

namespace IgdbApi.Lib
{
    // https://www.igdb.com/
    // https://api-docs.igdb.com/#authentication
    // "fields *; where id = (6,9,12); limit 500;"

    public class Igdb
    {
        private RestClient _client = new RestClient("https://api.igdb.com/v4");
        private Token _token = new Token();

        private const string _clientId = "PRIVATE";
        private const string _clientSecret = "PRIVATE";

        public void GetTwitchAccessToken()
        {
            RestRequest request = new RestRequest("https://id.twitch.tv/oauth2/token?client_id=" + _clientId + "&client_secret=" + _clientSecret + "&grant_type=client_credentials");
            RestResponse response = _client.Execute(request, Method.Post);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string rawResponse = response.Content;
                _token = JsonConvert.DeserializeObject<Token>(rawResponse);
            }
        }

        public void GetCoverArtUrl(string nameOfGame)
        {
            List<Game> gameResults = GetGames(nameOfGame);

            Game gameMatch = gameResults.Where(x => x.name.ToLower() == nameOfGame.ToLower()).FirstOrDefault();

            if (gameMatch != null)
            {
                List<GameDetails> gameIdResults = GetGameById(gameMatch.id.ToString());
                List<Cover> gameCovers = GetCover(gameIdResults[0].cover.ToString());
            }
        }

        public List<Game> GetGames(string nameOfGame)
        {
            RestRequest request = new RestRequest("/games");
            request.AddHeader("Client-ID", _clientId);
            request.AddHeader("Authorization", "Bearer " + _token.access_token);
            request.AddBody("fields name, involved_companies; search \"" + nameOfGame + "\";");

            RestResponse response = _client.Execute(request, Method.Post);

            List<Game> games = new List<Game>();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string rawResponse = response.Content;
                games = JsonConvert.DeserializeObject<List<Game>>(rawResponse);
            }

            return games;
        }

        public List<GameDetails> GetGameById(string gameId)
        {
            RestRequest request = new RestRequest("/games");
            request.AddHeader("Client-ID", _clientId);
            request.AddHeader("Authorization", "Bearer " + _token.access_token);
            request.AddBody("fields *; where id = " + gameId + ";");

            RestResponse response = _client.Execute(request, Method.Post);

            List<GameDetails> gameDetails = new List<GameDetails>();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string rawResponse = response.Content;
                gameDetails = JsonConvert.DeserializeObject<List<GameDetails>>(rawResponse);
            }

            return gameDetails;
        }

        public List<Cover> GetCover(string coverId)
        {
            RestRequest request = new RestRequest("/covers");
            request.AddHeader("Client-ID", _clientId);
            request.AddHeader("Authorization", "Bearer " + _token.access_token);
            request.AddBody("fields *; where id = " + coverId + ";");

            RestResponse response = _client.Execute(request, Method.Post);

            List<Cover> covers = new List<Cover>();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string rawResponse = response.Content;
                covers = JsonConvert.DeserializeObject<List<Cover>>(rawResponse);
            }

            return covers;
        }

        /// <summary>
        /// Gaming Platforms
        /// - Requires a limit set otherwise it just returns a default of 10 results only
        /// </summary>
        /// <param name="resultLimit"></param>
        public void GetPlatforms(int resultLimit = 500)
        {
            RestRequest request = new RestRequest("/platforms");
            request.AddHeader("Client-ID", _clientId);
            request.AddHeader("Authorization", "Bearer " + _token.access_token);
            request.AddBody("fields *; limit " + resultLimit + ";");

            RestResponse response = _client.Execute(request, Method.Post);

            List<Platform> platforms = new List<Platform>();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string rawResponse = response.Content;
                platforms = JsonConvert.DeserializeObject<List<Platform>>(rawResponse);
            }
        }

        /// <summary>
        /// Searches all endpoints
        /// - Search is usable on: Characters, Collections, Games, Platforms, and Themes
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="resultLimit"></param>
        public void Search(string keyword, int resultLimit = 50)
        {
            RestRequest request = new RestRequest("/search");
            request.AddHeader("Client-ID", _clientId);
            request.AddHeader("Authorization", "Bearer " + _token.access_token);
            request.AddBody("fields *; search \"" + keyword + "\"; limit " + resultLimit + ";");

            RestResponse response = _client.Execute(request, Method.Post);

            List<SearchResult> searchResult = new List<SearchResult>();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string rawResponse = response.Content;
                searchResult = JsonConvert.DeserializeObject<List<SearchResult>>(rawResponse);
            }
        }
    }
}