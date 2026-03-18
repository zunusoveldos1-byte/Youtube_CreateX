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
        public NewsModel News { get; set; }

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            News = await _context.News.FindAsync(id);
            if (News == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var newsToUpdate = await _context.News.FindAsync(News.Id);
            if (newsToUpdate == null)
                return NotFound();

            newsToUpdate.Title = News.Title;
            newsToUpdate.Content = News.Content;
            newsToUpdate.IsPublished = News.IsPublished;

            if (ImageFile != null)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                newsToUpdate.ImagePath = "/uploads/" + uniqueFileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}