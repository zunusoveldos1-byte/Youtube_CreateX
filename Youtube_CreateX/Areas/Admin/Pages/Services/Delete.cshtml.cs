using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Youtube_CreateX.Data;
using Youtube_CreateX.Models;

namespace Youtube_CreateX.Areas.Admin.Pages.Services
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
        public Service Service { get; set; } = new Service();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Service = await _context.Services.FindAsync(id);
            if (Service == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Service = await _context.Services.FindAsync(id);
            if (Service != null)
            {
                _context.Services.Remove(Service);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}