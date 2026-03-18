using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Youtube_CreateX.Data;
using Youtube_CreateX.Models;

namespace Youtube_CreateX.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Последние новости (3 шт)
        public List<Youtube_CreateX.Models.News> RecentNews { get; set; } = new();

        // Последние проекты (3 шт)
        public List<Project> RecentProjects { get; set; } = new();

        // Все партнёры (для бегущей строки)
        public List<Partner> Partners { get; set; } = new();

        // Услуги (для блока Our services)
        public List<Service> Services { get; set; } = new();

        public async Task OnGetAsync()
        {
            RecentNews = await _context.News
                .Where(n => n.IsPublished)
                .OrderByDescending(n => n.CreatedAt)
                .Take(3)
                .ToListAsync();

            RecentProjects = await _context.Projects
                .Where(p => p.IsPublished)
                .OrderByDescending(p => p.CreatedAt)
                .Take(3)
                .ToListAsync();

            Partners = await _context.Partners
                .Where(p => p.IsPublished)
                .OrderBy(p => p.DisplayOrder)
                .ToListAsync();

            Services = await _context.Services
                .Where(s => s.IsPublished)
                .OrderBy(s => s.DisplayOrder)
                .ToListAsync();
        }
    }
}