using BookSearchAPI.Models;
using System.Collections.Concurrent;

namespace BookSearchAPI.Services
{
    public class FavoriteService : IFavoriteService
    {
        // Thread-safe dictionary: Key = UserId, Value = List of Favorites
        private static readonly ConcurrentDictionary<string, List<FavoriteBook>> _storage = new();

        public bool AddFavorite(string userId, AddFavoriteRequest request)
        {
            var favorites = _storage.GetOrAdd(userId, _ => new List<FavoriteBook>());

            lock (favorites)
            {
                if (favorites.Any(f => f.ExternalId == request.ExternalId))
                {
                    return false; // Already exists
                }

                favorites.Add(new FavoriteBook
                {
                    UserId = userId,
                    ExternalId = request.ExternalId,
                    Title = request.Title,
                    Authors = request.Authors,
                    CoverUrl = request.CoverUrl,
                    AddedDate = DateTime.UtcNow
                });
                return true;
            }
        }

        public List<FavoriteResponse> GetFavorites(string userId)
        {
            if (_storage.TryGetValue(userId, out var favorites))
            {
                lock (favorites)
                {
                    return favorites.OrderByDescending(f => f.AddedDate)
                        .Select(f => new FavoriteResponse
                        {
                            ExternalId = f.ExternalId,
                            Title = f.Title,
                            Authors = f.Authors,
                            CoverUrl = f.CoverUrl,
                            AddedDate = f.AddedDate
                        }).ToList();
                }
            }
            return new List<FavoriteResponse>();
        }

        public bool IsFavorite(string userId, string externalId)
        {
            if (_storage.TryGetValue(userId, out var favorites))
            {
                lock (favorites)
                {
                    return favorites.Any(f => f.ExternalId == externalId);
                }
            }
            return false;
        }

        public bool RemoveFavorite(string userId, string externalId)
        {
            if (_storage.TryGetValue(userId, out var favorites))
            {
                lock (favorites)
                {
                    var item = favorites.FirstOrDefault(f => f.ExternalId == externalId);
                    if (item != null)
                    {
                        favorites.Remove(item);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
