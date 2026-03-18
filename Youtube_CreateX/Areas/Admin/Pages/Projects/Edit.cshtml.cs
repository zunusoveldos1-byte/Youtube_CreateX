using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Youtube_CreateX.Data;
using Youtube_CreateX.Models;

namespace Youtube_CreateX.Areas.Admin.Pages.Projects
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
        public Project Project { get; set; } = new Project();

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Project = await _context.Projects.FindAsync(id);
            if (Project == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var projectToUpdate = await _context.Projects.FindAsync(Project.Id);
            if (projectToUpdate == null)
                return NotFound();

            projectToUpdate.Title = Project.Title;
            projectToUpdate.Description = Project.Description;
            projectToUpdate.Client = Project.Client;
            projectToUpdate.CompletionDate = Project.CompletionDate;
            projectToUpdate.ProjectUrl = Project.ProjectUrl;
            projectToUpdate.Category = Project.Category;
            projectToUpdate.IsPublished = Project.IsPublished;

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
                projectToUpdate.ImagePath = "/uploads/" + uniqueFileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}