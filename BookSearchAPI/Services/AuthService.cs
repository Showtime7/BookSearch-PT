using BookSearchAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookSearchAPI.Services
{
    public class AuthService : IAuthService
    {
        // Clave secreta para firmar tokens (usar User Secrets en producción)
        public const string SecretKey = "SuperSecretKeyForBookSearchApp_MustBeLongEnough";
        
        private readonly BookSearchAPI.Data.ApplicationDbContext _context;

        public AuthService(BookSearchAPI.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public LoginResponse? Login(LoginRequest request)
        {
            // Busca usuario por nombre
            var user = _context.Users.SingleOrDefault(u => u.Username == request.Username);

            // Valida contraseña (usar hash real en producción)
            if (user != null && user.PasswordHash == request.Password)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(SecretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] 
                    { 
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim("UserId", user.Id.ToString()) // Agrega el ID como Claim
                    }),
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return new LoginResponse
                {
                    Username = user.Username,
                    Token = tokenHandler.WriteToken(token)
                };
            }
            return null;
        }
    }
}
