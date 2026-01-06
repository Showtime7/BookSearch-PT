using BookSearchAPI.Models;

namespace BookSearchAPI.Services
{
    public interface IAuthService
    {
        LoginResponse? Login(LoginRequest request);
    }
}
