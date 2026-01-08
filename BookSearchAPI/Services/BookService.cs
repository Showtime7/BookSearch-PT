using System.Text.Json;
using BookSearchAPI.Models;

namespace BookSearchAPI.Services
{
    // Servicio para interactuar con la API de OpenLibrary
    public class BookService : IBookService
    {
        private readonly HttpClient _httpClient;

        public BookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BookSearchResponse?> SearchBooksAsync(string query, int page = 1, int? year = null, string? genre = null)
        {
            // Construye la URL de búsqueda
            var url = $"https://openlibrary.org/search.json?q={Uri.EscapeDataString(query)}&page={page}";

            if (year.HasValue)
            {
                url += $"&first_publish_year={year.Value}";
            }
            
            // Agrega filtro de género si existe (usando 'subject')
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
