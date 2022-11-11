namespace TDM.Models
{
    public class UserLoginModel
    {
        public long UserId { get; set; }
        public string Password { get; set; } = null!;
        public string? UserName { get; set; } = null!;
        public string? RefreshToken { get; set; } = null!;
        public string? DisplayName { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public DateTime? CreatedDate { get; set; } = null!;
        public DateTime? UpdatedDate { get; set; } = null!;
    }
}