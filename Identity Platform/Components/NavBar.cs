namespace Identity.Platform.Components
{
    using System.Collections.Generic;
    using System.Linq;

    using Identity.Platform.Models;
    using Identity.Platform.Models.ViewModels;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewComponents;

    public class NavBar : ViewComponent
    {
        private static readonly Location[] Locations =
        {
            new Location("Home", "Index"),
            new Location("Users", "Index", "Regular Users Homepage"),
            new Location("Admins", "Index", "Admin Users Homepage")
        };

        public ViewViewComponentResult Invoke()
        {
            string activeController = ViewContext.RouteData.Values["Controller"].ToString();
            string activeAction = ViewContext.RouteData.Values["Action"].ToString();

            IEnumerable<NavLocation> navLocations = Locations.Select(location => new NavLocation(activeController == location.Controller && activeAction == location.Action, location));

            return View(navLocations);
        }
    }
}