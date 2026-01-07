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

        public LoginResponse? Login(LoginRequest request)
        {
            // Simple hardcoded validation
            if (request.Username == "admin" && request.Password == "admin123")
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(SecretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] 
                    { 
                        new Claim(ClaimTypes.Name, request.Username) 
                    }),
                    Expires = DateTime.UtcNow.AddHours(2), // 2 hours expiration
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return new LoginResponse
                {
                    Username = request.Username,
                    Token = tokenHandler.WriteToken(token)
                };
            }
            return null;
        }
    }
}
