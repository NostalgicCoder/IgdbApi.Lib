using IgdbApi.Lib.Class;
using IgdbApi.Lib.Models;
using Newtonsoft.Json;
using RestSharp;

namespace IgdbApi.Lib
{
    // https://www.igdb.com/
    // https://api-docs.igdb.com/#authentication

    public class Igdb
    {
        private ProcessImages _processImages = new ProcessImages();

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

        public FullGameData GetAllDataOnAGame(string nameOfGame)
        {
            FullGameData fullGameData = new FullGameData();

            List<Game> games = GetGamesByName(nameOfGame);

            // Check for a full name match
            fullGameData.Game = games.Where(x => x.name.ToLower() == nameOfGame.ToLower()).FirstOrDefault();

            if (fullGameData.Game == null)
            {
                // Check for a partial name match
                fullGameData.Game = games.Where(x => x.name.ToLower().Contains(nameOfGame.ToLower())).FirstOrDefault();
            }

            if (fullGameData.Game != null)
            {
                fullGameData.GameDetails = GetGameById(fullGameData.Game.id.ToString());

                if (fullGameData.Game.involved_companies != null)
                {
                    string involvedCompaniesIds = String.Join(",", fullGameData.Game.involved_companies);
                    fullGameData.InvolvedCompanies = GetInvolvedCompanies(involvedCompaniesIds);
                }

                if (fullGameData.GameDetails[0].platforms != null)
                {
                    string platformIds = String.Join(",", fullGameData.GameDetails[0].platforms);
                    fullGameData.Platforms = GetPlatforms(platformIds);
                }

                if (fullGameData.GameDetails[0].artworks != null)
                {
                    string artworkIds = String.Join(",", fullGameData.GameDetails[0].artworks);
                    fullGameData.Artworks = GetArtworks(artworkIds);
                }

                fullGameData.Covers = GetCover(fullGameData.GameDetails[0].cover.ToString());
                fullGameData.LargeCoverUrl = _processImages.ProcessCoverUrl(fullGameData.Covers[0].url);
            }

            return fullGameData;
        }

        /// <summary>
        /// Get basic data on a game by a search against its name
        /// - Calls the 'games' endpoint
        /// </summary>
        /// <param name="nameOfGame"></param>
        /// <returns></returns>
        public List<Game> GetGamesByName(string nameOfGame)
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

        /// <summary>
        /// Get various data on a game by a search against its ID
        /// - Calls the 'games' endpoint
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get the cover art of games
        /// - Calls the 'covers' endpoint
        /// </summary>
        /// <param name="coverId"></param>
        /// <returns></returns>
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
        /// - Calls the 'platforms' endpoint
        /// - Requires a limit set otherwise it just returns a default of 10 results only
        /// </summary>
        /// <param name="resultLimit"></param>
        public List<Platform> GetPlatforms(string platformIds, int resultLimit = 500)
        {
            RestRequest request = new RestRequest("/platforms");
            request.AddHeader("Client-ID", _clientId);
            request.AddHeader("Authorization", "Bearer " + _token.access_token);
            request.AddBody("fields *; where id = (" + platformIds + "); limit " + resultLimit + ";");

            RestResponse response = _client.Execute(request, Method.Post);

            List<Platform> platforms = new List<Platform>();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string rawResponse = response.Content;
                platforms = JsonConvert.DeserializeObject<List<Platform>>(rawResponse);
            }

            return platforms;
        }

        /// <summary>
        /// Gets information on the companies associated with a game
        /// - Calls the 'involved_companies' endpoint
        /// </summary>
        /// <param name="involvedCompaniesIds"></param>
        public List<InvolvedCompanies> GetInvolvedCompanies(string involvedCompaniesIds)
        {
            RestRequest request = new RestRequest("/involved_companies");
            request.AddHeader("Client-ID", _clientId);
            request.AddHeader("Authorization", "Bearer " + _token.access_token);
            request.AddBody("fields *; where id = (" + involvedCompaniesIds + ");");

            RestResponse response = _client.Execute(request, Method.Post);

            List<InvolvedCompanies> involvedCompanies = new List<InvolvedCompanies>();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string rawResponse = response.Content;
                involvedCompanies = JsonConvert.DeserializeObject<List<InvolvedCompanies>>(rawResponse);
            }

            return involvedCompanies;
        }

        /// <summary>
        /// Gets alternative artworks for the game
        /// - Calls the 'artworks' endpoint
        /// </summary>
        /// <param name="artworkIds"></param>
        /// <returns></returns>
        public List<Artworks> GetArtworks(string artworkIds)
        {
            RestRequest request = new RestRequest("/artworks");
            request.AddHeader("Client-ID", _clientId);
            request.AddHeader("Authorization", "Bearer " + _token.access_token);
            request.AddBody("fields *; where id = (" + artworkIds + ");");

            RestResponse response = _client.Execute(request, Method.Post);

            List<Artworks> artworks = new List<Artworks>();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string rawResponse = response.Content;
                artworks = JsonConvert.DeserializeObject<List<Artworks>>(rawResponse);
            }

            return artworks;
        }

        /// <summary>
        /// Searches all endpoints
        /// - Calls the 'search' endpoint
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