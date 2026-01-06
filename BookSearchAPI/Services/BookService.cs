using System.Text.Json;
using BookSearchAPI.Models;

namespace BookSearchAPI.Services
{
    public class BookService : IBookService
    {
        private readonly HttpClient _httpClient;

        public BookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BookSearchResponse?> SearchBooksAsync(string query, int page = 1, int? year = null, string? genre = null)
        {
            // Build URL
            // https://openlibrary.org/search.json?q=the+lord+of+the+rings&page=1
            var url = $"https://openlibrary.org/search.json?q={Uri.EscapeDataString(query)}&page={page}";

            if (year.HasValue)
            {
                url += $"&first_publish_year={year.Value}";
            }
            
            // Genre search in OpenLibrary is usually q=subject:genre or just implicit. 
            // The prompt asks for "Input de búsqueda por título o género". 
            // If the user inputs a genre in the main input, it's just 'q'.
            // If there is a separate filter for genre, we might append it.
            // Let's assume the query handles title/genre mixed or generic search.
            // But if 'genre' parameter is provided explicitly:
             if (!string.IsNullOrEmpty(genre))
            {
                url += $"&subject={Uri.EscapeDataString(genre)}";
            }

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var searchResponse = JsonSerializer.Deserialize<BookSearchResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return searchResponse;
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error fetching books: {ex.Message}");
                return null;
            }
        }
    }
}
