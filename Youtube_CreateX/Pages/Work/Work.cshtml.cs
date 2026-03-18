using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Youtube_CreateX.Data;
using Youtube_CreateX.Models;

namespace Youtube_CreateX.Pages.Work
{
    public class WorkModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public WorkModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Project> Projects { get; set; } = new List<Project>();
        public List<string> Categories { get; set; } = new List<string>();

        [BindProperty(SupportsGet = true)]
        public string? Category { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; }
        private const int PageSize = 9;

        public async Task OnGetAsync()
        {
            // ”никальные категории дл€ табов
            Categories = await _context.Projects
                .Where(p => p.IsPublished && p.Category != null)
                .Select(p => p.Category!)
                .Distinct()
                .ToListAsync();

            var query = _context.Projects.Where(p => p.IsPublished);

            if (!string.IsNullOrEmpty(Category))
            {
                query = query.Where(p => p.Category == Category);
            }

            int totalCount = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

            Projects = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}