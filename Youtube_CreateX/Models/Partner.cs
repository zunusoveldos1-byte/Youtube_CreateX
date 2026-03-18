using System.ComponentModel.DataAnnotations;

namespace Youtube_CreateX.Models
{
    public class Partner
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название компании обязательно")]
        [StringLength(200, ErrorMessage = "Максимум 200 символов")]
        public string Name { get; set; } = string.Empty;

        public string? LogoPath { get; set; }

        [Display(Name = "Веб-сайт")]
        [Url(ErrorMessage = "Некорректный URL")]
        public string? Website { get; set; }

        public string? Description { get; set; }

        [Display(Name = "Порядок отображения")]
        public int DisplayOrder { get; set; } = 0;

        [Display(Name = "Опубликовано")]
        public bool IsPublished { get; set; } = true;

        [Display(Name = "Дата создания")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}