using BookSearchAPI.Models;
using BookSearchAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookSearchAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // Servicio de autenticación inyectado
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Inicia sesión y retorna un token JWT válido
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

        // Cierra la sesión (simulado, ya que JWT es stateless)
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { message = "Logged out successfully" });
        }
    }
}
