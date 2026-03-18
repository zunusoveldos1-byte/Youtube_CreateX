using System.ComponentModel.DataAnnotations;   // обязательно!

namespace Youtube_CreateX.Models
{
    public class News
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string? ImagePath { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsPublished { get; set; }

        [StringLength(50)]
        public string? Category { get; set; }
    }
}