using BookSearchAPI.Models;
using BookSearchAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace BookSearchAPI.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly BookSearchAPI.Data.ApplicationDbContext _context;

        public FavoriteService(BookSearchAPI.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public bool AddFavorite(string userId, AddFavoriteRequest request)
        {
            if (!int.TryParse(userId, out int uid)) return false;

            // Check if already exists
            if (_context.Favorites.Any(f => f.UserId == uid && f.ExternalId == request.ExternalId))
            {
                return false;
            }

            var favorite = new Favorite
            {
                UserId = uid,
                ExternalId = request.ExternalId,
                Title = request.Title,
                Authors = string.Join(", ", request.Authors), // Convert List<string> to comma separated string
                CoverUrl = request.CoverUrl
                // FirstPublishYear not in request currently, optional
            };

            _context.Favorites.Add(favorite);
            _context.SaveChanges();
            return true;
        }

        public List<FavoriteResponse> GetFavorites(string userId)
        {
            if (!int.TryParse(userId, out int uid)) return new List<FavoriteResponse>();

            var favorites = _context.Favorites
                .Where(f => f.UserId == uid)
                .OrderByDescending(f => f.AddedDate)
                .ToList();

            return favorites.Select(f => new FavoriteResponse
            {
                ExternalId = f.ExternalId,
                Title = f.Title,
                Authors = f.Authors != null ? f.Authors.Split(", ", StringSplitOptions.None).ToList() : new List<string>(),
                CoverUrl = f.CoverUrl,
                AddedDate = f.AddedDate
            }).ToList();
        }

        public bool IsFavorite(string userId, string externalId)
        {
            if (!int.TryParse(userId, out int uid)) return false;

            return _context.Favorites.Any(f => f.UserId == uid && f.ExternalId == externalId);
        }

        public bool RemoveFavorite(string userId, string externalId)
        {
            if (!int.TryParse(userId, out int uid)) return false;

            var favorite = _context.Favorites.FirstOrDefault(f => f.UserId == uid && f.ExternalId == externalId);
            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
