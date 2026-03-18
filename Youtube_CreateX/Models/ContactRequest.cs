namespace Youtube_CreateX.Models
{
    public class ContactRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsProcessed { get; set; }
    }
}
