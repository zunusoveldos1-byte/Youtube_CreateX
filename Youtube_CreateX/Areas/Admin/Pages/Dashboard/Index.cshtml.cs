using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Youtube_CreateX.Data;

namespace Youtube_CreateX.Areas.Admin.Pages.Dashboard
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Общие счётчики
        public int UsersCount { get; set; }
        public int NewsCount { get; set; }
        public int ServicesCount { get; set; }
        public int ProjectsCount { get; set; }
        public int PartnersCount { get; set; }

        // Категории проектов с количеством
        public Dictionary<string, int> ProjectCategories { get; set; } = new();

        // Категории новостей с количеством
        public Dictionary<string, int> NewsCategories { get; set; } = new();

        // Последние добавленные записи (опционально)
        public List<Youtube_CreateX.Models.Project> RecentProjects { get; set; } = new();
        public List<Youtube_CreateX.Models.News> RecentNews { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Основные счётчики
            UsersCount = await _context.Users.CountAsync();
            NewsCount = await _context.News.CountAsync();
            ServicesCount = await _context.Services.CountAsync();
            ProjectsCount = await _context.Projects.CountAsync();
            PartnersCount = await _context.Partners.CountAsync();

            // Группировка проектов по категориям
            ProjectCategories = await _context.Projects
                .Where(p => p.IsPublished && p.Category != null)
                .GroupBy(p => p.Category!)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.Category, g => g.Count);

            // Группировка новостей по категориям
            NewsCategories = await _context.News
                .Where(n => n.IsPublished && n.Category != null)
                .GroupBy(n => n.Category!)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.Category, g => g.Count);

            // Последние 5 проектов
            RecentProjects = await _context.Projects
                .Where(p => p.IsPublished)
                .OrderByDescending(p => p.CreatedAt)
                .Take(5)
                .ToListAsync();

            // Последние 5 новостей
            RecentNews = await _context.News
                .Where(n => n.IsPublished)
                .OrderByDescending(n => n.CreatedAt)
                .Take(5)
                .ToListAsync();
        }
    }
}