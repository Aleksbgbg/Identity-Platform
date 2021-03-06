﻿namespace Identity.Platform.Components
{
    using Identity.Platform.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewComponents;

    public class UserTable : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public UserTable(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public ViewViewComponentResult Invoke()
        {
            return View(_userManager.Users);
        }
    }
}