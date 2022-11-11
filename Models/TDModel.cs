namespace TDM.Models
{
    public class TDModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string ModelName { get; set; } = null!;
        public IFormFile ModelIFormFile { get; set; } = null!;
        public Byte[]? Model { get; set; }
        public string Email { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}