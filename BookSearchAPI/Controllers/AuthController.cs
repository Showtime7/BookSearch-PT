using BookSearchAPI.Models;
using BookSearchAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookSearchAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var response = _authService.Login(request);
            if (response == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }
            return Ok(response);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Stateless JWT/Token usually means logout is client-side, 
            // but we can have an endpoint if needed for cookies or future extension.
            return Ok(new { message = "Logged out successfully" });
        }
    }
}
