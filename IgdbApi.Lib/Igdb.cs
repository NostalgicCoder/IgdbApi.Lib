﻿using IgdbApi.Lib.Class;
using IgdbApi.Lib.Interfaces;
using IgdbApi.Lib.Models;
using Newtonsoft.Json;
using RestSharp;

namespace IgdbApi.Lib
{
    public class Igdb : IIgdb
    {
        private ProcessImages _processImages = new ProcessImages();
        private SearchForGame _searchForGame = new SearchForGame();
        private Token _token = new Token();
        private RestClient _client = new RestClient("https://api.igdb.com/v4");

        private string _clientId;

        /// <summary>
        /// This is called by either the 'TestHarness.Console' OR 'TestHarness.Visual' project - One is better for speed based API testing and one is better for pre-viewing the visual data in a real world scenario
        /// - If parent level data can be aquired from the API then call child requests to aquire more detailed information for the result
        /// </summary>
        /// <param name="nameOfGame"></param>
        /// <param name="platformId"></param>
        /// <returns></returns>
        public IFullGameData GetAllDataOnAGame(string nameOfGame, int platformId = 0)
        {
            try
            {
                IFullGameData fullGameData = new FullGameData();

                List<IgdbGame> games = GetGamesByName(nameOfGame);

                fullGameData.Game = _searchForGame.SearchForGameByNameAndPlatform(games, nameOfGame, platformId);

                if (fullGameData.Game != null)
                {
                    fullGameData.GameDetails = GetGameById(fullGameData.Game.id.ToString());

                    fullGameData.ReleaseDates = GetReleaseDates(fullGameData.Game.id.ToString(), platformId);

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

                    if (fullGameData.GameDetails[0].genres != null)
                    {
                        string genreIds = String.Join(",", fullGameData.GameDetails[0].genres);
                        fullGameData.Genres = GetGenres(genreIds);
                    }

                    if (fullGameData.GameDetails[0].artworks != null)
                    {
                        string artworkIds = String.Join(",", fullGameData.GameDetails[0].artworks);
                        fullGameData.Artworks = GetArtworks(artworkIds);
                        fullGameData.LargeArtworkUrls = _processImages.ProcessArrayOfImages(fullGameData.Artworks);
                    }

                    if (fullGameData.GameDetails[0].cover != 0)
                    {
                        fullGameData.Covers = GetCover(fullGameData.GameDetails[0].cover.ToString());
                        fullGameData.LargeCoverUrl = _processImages.ProcessCoverUrl(fullGameData.Covers[0].url);
                    }
                }

                return fullGameData;
            }
            catch (Exception ex)
            {
                // TODO: A more detailed error response can be feedback here in the future to calling applications, for now this is fine.
                return null;
            }
        }

        /// <summary>
        /// Get the access token from Twitch that is required for access to IGDB API
        /// - Pass over 'clientId' and 'clientSecret' that is unique to the users account
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        public void GetTwitchAccessToken(string clientId, string clientSecret)
        {
            RestRequest request = new RestRequest("https://id.twitch.tv/oauth2/token?client_id=" + clientId + "&client_secret=" + clientSecret + "&grant_type=client_credentials");
            RestResponse response = _client.Execute(request, Method.Post);

            _clientId = clientId;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string rawResponse = response.Content;
                _token = JsonConvert.DeserializeObject<Token>(rawResponse);
            }
        }

        /// <summary>
        /// Call the IGDB (Internet Gaming Database) API
        /// - Pass over the requests command via the 'headerbody' value and return the response as a string
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="headerBody"></param>
        /// <returns></returns>
        private string CallIgdbApi(string endpoint, string headerBody)
        {
            RestRequest request = new RestRequest(endpoint);
            request.AddHeader("Client-ID", _clientId);
            request.AddHeader("Authorization", "Bearer " + _token.access_token);
            request.AddBody(headerBody);

            RestResponse response = _client.Execute(request, Method.Post);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.Content;
            }

            return null; ;
        }

        /// <summary>
        /// Get basic data on a game by a search against its name
        /// - Calls the 'games' endpoint
        /// </summary>
        /// <param name="nameOfGame"></param>
        /// <returns></returns>
        public List<IgdbGame> GetGamesByName(string nameOfGame)
        {
            List<IgdbGame> games = new List<IgdbGame>();

            string response = CallIgdbApi(Endpoints.Games, "fields name, platforms, involved_companies; search \"" + nameOfGame + "\";");

            games = JsonConvert.DeserializeObject<List<IgdbGame>>(response);

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
            List<GameDetails> gameDetails = new List<GameDetails>();

            string response = CallIgdbApi(Endpoints.Games, "fields *; where id = " + gameId + ";");

            gameDetails = JsonConvert.DeserializeObject<List<GameDetails>>(response);

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
            List<Cover> covers = new List<Cover>();

            string response = CallIgdbApi(Endpoints.Covers, "fields *; where id = " + coverId + ";");

            covers = JsonConvert.DeserializeObject<List<Cover>>(response);

            return covers;
        }


        /// <summary>
        /// Calls the 'platforms' endpoint
        /// - Requires a limit set otherwise it just returns a default of 10 results only
        /// </summary>
        /// <param name="platformIds"></param>
        /// <param name="resultLimit"></param>
        /// <returns></returns>
        public List<Platform> GetPlatforms(string platformIds, int resultLimit = 500)
        {
            List<Platform> platforms = new List<Platform>();

            string response = CallIgdbApi(Endpoints.Platforms, "fields *; where id = (" + platformIds + "); limit " + resultLimit + ";");

            platforms = JsonConvert.DeserializeObject<List<Platform>>(response);

            return platforms;
        }

        /// <summary>
        /// Gets information on the companies associated with a game
        /// - Calls the 'involved_companies' endpoint
        /// </summary>
        /// <param name="involvedCompaniesIds"></param>
        /// <returns></returns>
        public List<InvolvedCompanies> GetInvolvedCompanies(string involvedCompaniesIds)
        {
            List<InvolvedCompanies> involvedCompanies = new List<InvolvedCompanies>();

            string response = CallIgdbApi(Endpoints.InvolvedCompanies, "fields *; where id = (" + involvedCompaniesIds + ");");

            involvedCompanies = JsonConvert.DeserializeObject<List<InvolvedCompanies>>(response);

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
            List<Artworks> artworks = new List<Artworks>();

            string response = CallIgdbApi(Endpoints.Artworks, "fields *; where id = (" + artworkIds + ");");

            artworks = JsonConvert.DeserializeObject<List<Artworks>>(response);

            return artworks;
        }

        /// <summary>
        /// Searches all endpoints
        /// - Calls the 'search' endpoint
        /// - Search is usable on: Characters, Collections, Games, Platforms, and Themes
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="resultLimit"></param>
        /// <returns></returns>
        public List<SearchResult> Search(string keyword, int resultLimit = 50)
        {
            List<SearchResult> searchResult = new List<SearchResult>();

            string response = CallIgdbApi(Endpoints.Search, "fields *; search \"" + keyword + "\"; limit " + resultLimit + ";");

            searchResult = JsonConvert.DeserializeObject<List<SearchResult>>(response);

            return searchResult;
        }

        /// <summary>
        /// - Calls the 'genre' endpoint
        /// </summary>
        /// <param name="genreIds"></param>
        /// <returns></returns>
        public List<Genre> GetGenres(string genreIds)
        {
            List<Genre> genres = new List<Genre>();

            string response = CallIgdbApi(Endpoints.Genres, "fields *; where id = (" + genreIds + ");");

            genres = JsonConvert.DeserializeObject<List<Genre>>(response);

            return genres;
        }

        /// <summary>
        /// Returns release date information for a game
        /// - Calls the 'release_dates' endpoint
        /// - Sometimes you will get multiple release dates returned for the same game and platform due to different editions of the game being released, IE: Deluxe, Regular
        /// - Appears some date values returned from IGDB API are in millisecond date format, so are not human readable without conversion.
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="platformId"></param>
        /// <returns></returns>
        public List<ReleaseDates> GetReleaseDates(string gameId, int platformId)
        {
            List<ReleaseDates> releaseDates = new List<ReleaseDates>();

            string headerBody = string.Empty;

            if (platformId == 0)
            {
                headerBody = "fields *; where game = " + gameId + ";";
            }
            else
            {
                headerBody = "fields *; where game = " + gameId + "& platform = " + platformId.ToString() + ";";
            }

            string response = CallIgdbApi(Endpoints.ReleaseDates, headerBody);

            releaseDates = JsonConvert.DeserializeObject<List<ReleaseDates>>(response);

            return releaseDates;
        }
    }
}