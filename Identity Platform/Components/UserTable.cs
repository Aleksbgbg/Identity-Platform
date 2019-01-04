namespace Identity.Platform.Components
{
    using Identity.Platform.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UserTable : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public UserTable(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            return View(_userManager.Users);
        }
    }
}