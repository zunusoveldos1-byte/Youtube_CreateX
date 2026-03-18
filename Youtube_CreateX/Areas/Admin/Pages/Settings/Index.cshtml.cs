using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Youtube_CreateX.Data;
using Youtube_CreateX.Models;

namespace Youtube_CreateX.Areas.Admin.Pages.Settings
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string SiteName { get; set; }

        [BindProperty]
        public string ContactEmail { get; set; }

        [BindProperty]
        public string Phone { get; set; }

        public async Task OnGetAsync()
        {
            SiteName = await GetSetting("SiteName");
            ContactEmail = await GetSetting("ContactEmail");
            Phone = await GetSetting("Phone");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await SaveSetting("SiteName", SiteName);
            await SaveSetting("ContactEmail", ContactEmail);
            await SaveSetting("Phone", Phone);
            TempData["StatusMessage"] = "Настройки сохранены";
            return RedirectToPage();
        }

        private async Task<string> GetSetting(string key)
        {
            var setting = await _context.SiteSettings.FirstOrDefaultAsync(s => s.Key == key);
            return setting?.Value ?? "";
        }

        private async Task SaveSetting(string key, string value)
        {
            var setting = await _context.SiteSettings.FirstOrDefaultAsync(s => s.Key == key);
            if (setting == null)
            {
                _context.SiteSettings.Add(new SiteSettings { Key = key, Value = value });
            }
            else
            {
                setting.Value = value;
            }
            await _context.SaveChangesAsync();
        }
    }
}