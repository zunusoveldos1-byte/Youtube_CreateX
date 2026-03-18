using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Youtube_CreateX.Pages
{
    public class profileModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<profileModel> _logger;

        public profileModel(UserManager<IdentityUser> userManager, ILogger<profileModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public IFormFile? AvatarFile { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Email = user.Email;
            Name = user.UserName; // или дополнительное поле, если вы его добавили

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Обновляем email, если он изменился
            if (Email != user.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Email);
                if (!setEmailResult.Succeeded)
                {
                    foreach (var error in setEmailResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }
            }

            // Обновляем имя (UserName)
            if (Name != user.UserName)
            {
                var setUserNameResult = await _userManager.SetUserNameAsync(user, Name);
                if (!setUserNameResult.Succeeded)
                {
                    foreach (var error in setUserNameResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }
            }

            // Обработка загрузки аватара (требует расширения модели User)
            if (AvatarFile != null)
            {
                // Пример сохранения файла (нужно добавить поле AvatarPath в IdentityUser)
                // var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                // var uniqueFileName = Guid.NewGuid().ToString() + "_" + AvatarFile.FileName;
                // var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                // using (var fileStream = new FileStream(filePath, FileMode.Create))
                // {
                //     await AvatarFile.CopyToAsync(fileStream);
                // }
                // user.AvatarPath = "/uploads/" + uniqueFileName; // требуется расширение модели
            }

            await _userManager.UpdateAsync(user);

            // Можно вернуть JSON для AJAX-запроса или просто перенаправить
            return RedirectToPage();
        }
    }
}