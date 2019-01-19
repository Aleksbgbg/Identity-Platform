namespace Identity.Platform.Components
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Identity.Platform.Models;
    using Identity.Platform.Models.ViewModels;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewComponents;

    public class NavBar : ViewComponent
    {
        private static readonly Location[] DefaultLocations =
        {
            new Location("Home", "Index"),
            new Location("User", "Index", "User Homepage"),
            new Location("Admin", "Index", "Admin Homepage")
        };

        private readonly UserManager<AppUser> _userManager;

        public NavBar(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ViewViewComponentResult> InvokeAsync(Location[] locations)
        {
            string activeController = ViewContext.RouteData.Values["Controller"].ToString();
            string activeAction = ViewContext.RouteData.Values["Action"].ToString();

            IEnumerable<NavLocation> navLocations = (locations ?? DefaultLocations).Select
            (
                location => new NavLocation
                (
                    activeController == location.Controller && activeAction == location.Action,
                    location
                )
           );

            return View(new NavbarModel(navLocations, await _userManager.GetUserAsync(UserClaimsPrincipal)));
        }
    }
}