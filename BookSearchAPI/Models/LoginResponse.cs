namespace BookSearchAPI.Models
{
    public class LoginResponse
    {
        public required string Username { get; set; }
        public required string Token { get; set; } // Just a dummy token or username for now
    }
}
