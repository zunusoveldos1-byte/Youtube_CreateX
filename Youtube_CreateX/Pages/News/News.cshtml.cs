using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Youtube_CreateX.Data;
using Youtube_CreateX.Models;

namespace Youtube_CreateX.Pages.News
{
    public class NewsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public NewsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Список новостей для отображения
        public IList<Youtube_CreateX.Models.News> NewsList { get; set; } = new List<Youtube_CreateX.Models.News>();

        // Доступные категории (для кнопок фильтрации)
        public List<string> Categories { get; set; } = new List<string>();

        // Текущая выбранная категория (приходит из query string)
        [BindProperty(SupportsGet = true)]
        public string? Category { get; set; }

        // Параметры пагинации
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; }
        private const int PageSize = 6; // Количество новостей на странице

        public async Task OnGetAsync()
        {
            // Базовый запрос: только опубликованные новости
            var query = _context.News.Where(n => n.IsPublished);

            // Получаем все уникальные категории (не null) для кнопок фильтра
            Categories = await _context.News
                .Where(n => n.IsPublished && n.Category != null)
                .Select(n => n.Category!)
                .Distinct()
                .ToListAsync();

            // Если выбрана категория – фильтруем
            if (!string.IsNullOrEmpty(Category))
            {
                query = query.Where(n => n.Category == Category);
            }

            // Подсчёт общего количества элементов для пагинации
            int totalCount = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

            // Получаем элементы для текущей страницы
            NewsList = await query
                .OrderByDescending(n => n.CreatedAt)
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}

