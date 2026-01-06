using BookSearchAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookSearchAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query, [FromQuery] int page = 1, [FromQuery] int? year = null, [FromQuery] string? genre = null)
        {
            if (string.IsNullOrWhiteSpace(query) && string.IsNullOrWhiteSpace(genre)) 
            {
                return BadRequest(new { message = "Query or genre is required" });
            }

            // If query is empty but genre is provided, use genre as query or handle in service.
            // Service handles it by appending subject if genre is there, but main query might be empty?
            // OpenLibrary allows empty 'q' if other params are there? Let's assume yes or rely on service logic.
            // But let's pass "" if null.
           var safeQuery = query ?? "";

            var result = await _bookService.SearchBooksAsync(safeQuery, page, year, genre);
            if (result == null)
            {
                return StatusCode(500, new { message = "Error fetching data from Open Library" });
            }

            return Ok(result);
        }
    }
}
