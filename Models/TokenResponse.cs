namespace TDM.Models
{
    public class TokenResponse
    {
        public long UserId { get; set; }
        public string RefreshToken { get; set; } = null!;
        public string JWTToken { get; set; } = null!;
    }
}