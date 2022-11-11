namespace TDM.DTOs
{
    public class TDModelDTO
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public string? ModelName { get; set; }
        public IFormFile? ModelIFormFile { get; set; }
        public Byte[]? Model { get; set; }
        public string? Email { get; set; } = null!;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}