using System.Text.Json.Serialization;

namespace BookSearchAPI.Models
{
    public class BookSearchResponse
    {
        [JsonPropertyName("num_found")]
        public int NumFound { get; set; }

        [JsonPropertyName("docs")]
        public List<BookDoc> Docs { get; set; } = new List<BookDoc>();
    }

    public class BookDoc
    {
        [JsonPropertyName("key")]
        public string Key { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("author_name")]
        public List<string>? AuthorName { get; set; }

        [JsonPropertyName("first_publish_year")]
        public int? FirstPublishYear { get; set; }

        [JsonPropertyName("cover_i")]
        public int? CoverId { get; set; }
        
        // Helper to get cover URL
        public string CoverUrl => CoverId.HasValue 
            ? $"https://covers.openlibrary.org/b/id/{CoverId}-M.jpg" 
            : "assets/placeholder-book.png"; 
    }
}
