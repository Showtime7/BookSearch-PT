using System.Text.Json.Serialization;

namespace BookSearchAPI.Models
{
    public class FavoriteBook
    {
        public string ExternalId { get; set; } = string.Empty; // OpenLibrary Key
        public string Title { get; set; } = string.Empty;
        public List<string> Authors { get; set; } = new List<string>();
        public string? CoverUrl { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; } = string.Empty; // Associated User
    }

    public class AddFavoriteRequest
    {
        public string ExternalId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<string> Authors { get; set; } = new List<string>();
        public string? CoverUrl { get; set; }
    }

    public class FavoriteResponse
    {
        public string ExternalId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<string> Authors { get; set; } = new List<string>();
        public string? CoverUrl { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
