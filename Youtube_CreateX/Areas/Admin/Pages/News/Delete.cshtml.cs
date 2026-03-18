using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Youtube_CreateX.Data;
using Youtube_CreateX.Models;
using NewsModel = Youtube_CreateX.Models.News;

namespace Youtube_CreateX.Areas.Admin.Pages.News
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public NewsModel News { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            News = await _context.News.FindAsync(id);
            if (News == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            News = await _context.News.FindAsync(id);
            if (News != null)
            {
                _context.News.Remove(News);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}