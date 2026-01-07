using BookSearchAPI.Models;

namespace BookSearchAPI.Services
{
    public interface IFavoriteService
    {
        bool AddFavorite(string userId, AddFavoriteRequest request);
        List<FavoriteResponse> GetFavorites(string userId);
        bool RemoveFavorite(string userId, string externalId);
        bool IsFavorite(string userId, string externalId);
    }
}
