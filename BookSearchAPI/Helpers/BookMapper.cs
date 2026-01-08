using BookSearchAPI.Models;
using System.Security.AccessControl;

namespace BookSearchAPI.Helpers
{
    public static class BookMapper
    {
        // Convierte un documento de Open Library a un objeto Favorite
        public static AddFavoriteRequest MapToFavoriteRequest(BookDoc doc)
        {
            return new AddFavoriteRequest
            {
                ExternalId = doc.Key ?? string.Empty,
                Title = doc.Title ?? "Unknown Title",
                Authors = doc.AuthorName != null ? doc.AuthorName : new List<string>(),
                CoverUrl = doc.CoverId > 0 ? $"https://covers.openlibrary.org/b/id/{doc.CoverId}-M.jpg" : null
            };
        }
    }
}
