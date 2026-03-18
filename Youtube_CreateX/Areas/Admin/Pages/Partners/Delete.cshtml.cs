using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Youtube_CreateX.Data;
using Youtube_CreateX.Models;

namespace Youtube_CreateX.Areas.Admin.Pages.Partners
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
        public Partner Partner { get; set; } = new Partner();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Partner = await _context.Partners.FindAsync(id);
            if (Partner == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Partner = await _context.Partners.FindAsync(id);
            if (Partner != null)
            {
                _context.Partners.Remove(Partner);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}