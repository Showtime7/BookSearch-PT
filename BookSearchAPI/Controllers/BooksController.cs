using BookSearchAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookSearchAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Controlador para gestionar la búsqueda de libros
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // Busca libros en la API externa aplicando filtros
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query, [FromQuery] int page = 1, [FromQuery] int? year = null, [FromQuery] string? genre = null)
        {
            if (string.IsNullOrWhiteSpace(query) && string.IsNullOrWhiteSpace(genre)) 
            {
                return BadRequest(new { message = "Query or genre is required" });
            }

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
