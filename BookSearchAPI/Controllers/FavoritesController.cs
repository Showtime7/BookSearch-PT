using BookSearchAPI.Models;
using BookSearchAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookSearchAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requiere autenticación
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoritesController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        // Obtiene el ID del usuario actual desde las Claims del Token
        private string GetCurrentUserId()
        {
            return User.FindFirst("UserId")?.Value ?? string.Empty;
        }

        // Agrega un nuevo libro a favoritos
        [HttpPost]
        public IActionResult AddFavorite([FromBody] AddFavoriteRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetCurrentUserId();
            var success = _favoriteService.AddFavorite(userId, request);

            if (!success)
            {
                return Conflict(new { message = "Este libro ya está en tus favoritos" });
            }

            return Ok(new { message = "Added to favorites" });
        }

        // Obtiene la lista de favoritos del usuario
        [HttpGet]
        public IActionResult GetFavorites()
        {
            var userId = GetCurrentUserId();
            var list = _favoriteService.GetFavorites(userId);
            return Ok(list);
        }

        // Elimina un libro de favoritos
        [HttpDelete]
        public IActionResult RemoveFavorite([FromQuery] string externalId)
        {
            var userId = GetCurrentUserId();
            var success = _favoriteService.RemoveFavorite(userId, externalId);
            
            if (!success)
            {
                return NotFound(new { message = "Favorite not found" });
            }

            return Ok(new { message = "Removed from favorites" });
        }

        // Verifica si un libro específico ya es favorito
        [HttpGet("check/{externalId}")]
        public IActionResult IsFavorite(string externalId)
        {
            var userId = GetCurrentUserId();
            var isFav = _favoriteService.IsFavorite(userId, externalId);
            return Ok(isFav);
        }
    }
}
