using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Youtube_CreateX.Data;
using Youtube_CreateX.Models;

namespace Youtube_CreateX.Areas.Admin.Pages.Services
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EditModel(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public Service Service { get; set; } = new Service();

        [BindProperty]
        public IFormFile? IconFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Service = await _context.Services.FindAsync(id);
            if (Service == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var serviceToUpdate = await _context.Services.FindAsync(Service.Id);
            if (serviceToUpdate == null)
                return NotFound();

            serviceToUpdate.Title = Service.Title;
            serviceToUpdate.ShortDescription = Service.ShortDescription;
            serviceToUpdate.FullDescription = Service.FullDescription;
            serviceToUpdate.DisplayOrder = Service.DisplayOrder;
            serviceToUpdate.IsPublished = Service.IsPublished;

            if (IconFile != null)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(IconFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await IconFile.CopyToAsync(stream);
                }
                serviceToUpdate.IconPath = "/uploads/" + uniqueFileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}