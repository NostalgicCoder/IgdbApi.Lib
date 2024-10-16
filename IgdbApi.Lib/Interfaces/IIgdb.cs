using IgdbApi.Lib.Models;

namespace IgdbApi.Lib.Interfaces
{
    public interface IIgdb
    {
        IFullGameData GetAllDataOnAGame(string nameOfGame, int platformId = 0);
        void GetTwitchAccessToken(string clientId, string clientSecret);
        List<IgdbGame> GetGamesByName(string nameOfGame);
        List<GameDetails> GetGameById(string gameId);
        List<Cover> GetCover(string coverId);
        List<Platform> GetPlatforms(string platformIds, int resultLimit = 500);
        List<InvolvedCompanies> GetInvolvedCompanies(string involvedCompaniesIds);
        List<Artworks> GetArtworks(string artworkIds);
        List<SearchResult> Search(string keyword, int resultLimit = 50);
        List<Genre> GetGenres(string genreIds);
        List<ReleaseDates> GetReleaseDates(string gameId, int platformId);
    }
}