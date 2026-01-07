using BookSearchAPI.Models;
using BookSearchAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookSearchAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // All endpoints require login
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoritesController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        private string GetCurrentUserId()
        {
            // Extract username from Claims
            return User.FindFirst(ClaimTypes.Name)?.Value ?? "anonymous";
        }

        [HttpPost]
        public IActionResult AddFavorite([FromBody] AddFavoriteRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetCurrentUserId();
            var success = _favoriteService.AddFavorite(userId, request);

            if (!success)
            {
                return Conflict(new { message = "Book already in favorites" }); // 409
            }

            return Ok(new { message = "Added to favorites" });
        }

        [HttpGet]
        public IActionResult GetFavorites()
        {
            var userId = GetCurrentUserId();
            var list = _favoriteService.GetFavorites(userId);
            return Ok(list);
        }

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

        [HttpGet("check/{externalId}")]
        public IActionResult IsFavorite(string externalId)
        {
            var userId = GetCurrentUserId();
            var isFav = _favoriteService.IsFavorite(userId, externalId);
            return Ok(isFav);
        }
    }
}
