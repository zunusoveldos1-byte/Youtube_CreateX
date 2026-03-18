using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Youtube_CreateX.Data;
using Youtube_CreateX.Models;
using NewsModel = Youtube_CreateX.Models.News;

namespace Youtube_CreateX.Areas.Admin.Pages.News
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<NewsModel> NewsList { get; set; }

        public async Task OnGetAsync()
        {
            NewsList = await _context.News.OrderByDescending(n => n.CreatedAt).ToListAsync();
        }
    }
}