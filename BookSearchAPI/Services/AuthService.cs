using BookSearchAPI.Models;

namespace BookSearchAPI.Services
{
    public class AuthService : IAuthService
    {
        public LoginResponse? Login(LoginRequest request)
        {
            if (request.Username == "admin" && request.Password == "admin123")
            {
                return new LoginResponse
                {
                    Username = request.Username,
                    Token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Guid.NewGuid().ToString())) // Dummy token
                };
            }
            return null;
        }
    }
}
