namespace TDM.DTOs
{    
    public class TDModelDTOPost
    {
        public IFormFile ModelIFormFile { get; set; } = null!;
        public long UserId { get; set; }
        public string Email { get; set; } = null!;
    }
}