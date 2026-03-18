using System.ComponentModel.DataAnnotations;

namespace Youtube_CreateX.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название обязательно")]
        [StringLength(200, ErrorMessage = "Максимум 200 символов")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Максимум 500 символов")]
        public string? ShortDescription { get; set; }

        public string? FullDescription { get; set; }

        public string? IconPath { get; set; }

        [Display(Name = "Порядок отображения")]
        public int DisplayOrder { get; set; } = 0;

        [Display(Name = "Опубликовано")]
        public bool IsPublished { get; set; } = true;

        [Display(Name = "Дата создания")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}