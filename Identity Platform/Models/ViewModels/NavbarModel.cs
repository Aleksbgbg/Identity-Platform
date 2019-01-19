namespace Identity.Platform.Models.ViewModels
{
    using System.Collections.Generic;

    public class NavbarModel
    {
        public NavbarModel(IEnumerable<NavLocation> navLocations, AppUser currentUser)
        {
            NavLocations = navLocations;
            CurrentUser = currentUser;
        }

        public IEnumerable<NavLocation> NavLocations { get; }

        public AppUser CurrentUser { get; }
    }
}