namespace TDM.Models
{
    public partial class TblRefreshtoken
    {
        public long RefreshTokenId { get; set; }
        public long UserId { get; set; }
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public bool? IsActive { get; set; }
    }
}