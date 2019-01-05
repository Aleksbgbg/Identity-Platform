namespace Identity.Platform.Components
{
    using System.Threading.Tasks;

    using Identity.Platform.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UserPropertyTable : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public UserPropertyTable(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _userManager.GetUserAsync(HttpContext.User));
        }
    }
}