using System.ComponentModel.DataAnnotations;

namespace Youtube_CreateX.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название обязательно")]
        [StringLength(200, ErrorMessage = "Максимум 200 символов")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Display(Name = "Клиент")]
        public string? Client { get; set; }

        [Display(Name = "Дата завершения")]
        [DataType(DataType.Date)]
        public DateTime? CompletionDate { get; set; }

        [Display(Name = "Ссылка на проект")]
        [Url(ErrorMessage = "Некорректный URL")]
        public string? ProjectUrl { get; set; }

        public string? ImagePath { get; set; }

        [Display(Name = "Категория")]
        public string? Category { get; set; }

        [Display(Name = "Опубликовано")]
        public bool IsPublished { get; set; } = true;

        [Display(Name = "Дата создания")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}