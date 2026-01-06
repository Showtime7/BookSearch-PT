using BookSearchAPI.Models;

namespace BookSearchAPI.Services
{
    public interface IBookService
    {
        Task<BookSearchResponse?> SearchBooksAsync(string query, int page = 1, int? year = null, string? genre = null);
    }
}
