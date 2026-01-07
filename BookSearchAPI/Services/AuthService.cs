using BookSearchAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookSearchAPI.Services
{
    public class AuthService : IAuthService
    {
        // For POC only. In production, use User Secrets or Environment Variables.
        public const string SecretKey = "SuperSecretKeyForBookSearchApp_MustBeLongEnough";
        
        private readonly BookSearchAPI.Data.ApplicationDbContext _context;

        public AuthService(BookSearchAPI.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public LoginResponse? Login(LoginRequest request)
        {
            // Find user by username
            var user = _context.Users.SingleOrDefault(u => u.Username == request.Username);

            // Validate user and password (PasswordHash is currently plain text based on seed data)
            // In a real app, use BCrypt.Verify(request.Password, user.PasswordHash)
            if (user != null && user.PasswordHash == request.Password)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(SecretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] 
                    { 
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim("UserId", user.Id.ToString()) // Add UserId claim
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
