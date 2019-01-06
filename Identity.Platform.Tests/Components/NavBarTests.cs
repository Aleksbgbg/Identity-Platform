namespace Identity.Platform.Tests.Components
{
    using System.Collections.Generic;
    using System.Linq;

    using Identity.Platform.Components;
    using Identity.Platform.Models;
    using Identity.Platform.Models.ViewModels;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewComponents;
    using Microsoft.AspNetCore.Routing;

    using Xunit;

    public class NavBarTests
    {
        [Fact]
        public void SelectsCorrectLocationsAsActive()
        {
            // Arrange
            NavBar navBar = new NavBar
            {
                ViewComponentContext = new ViewComponentContext
                {
                    ViewContext = new ViewContext
                    {
                        RouteData = new RouteData
                        {
                            Values =
                            {
                                ["Controller"] = "Home",
                                ["Action"] = "Index"
                            }
                        }
                    }
                }
            };

            Location[] locations =
            {
                new Location("Home", "Index"),
                new Location("Users", "Index", "Regular Users Homepage"),
                new Location("Admins", "Index", "Admin Users Homepage")
            };

            // Act
            ViewViewComponentResult view = navBar.Invoke(locations);

            // Assert
            object viewModel = view.ViewData.Model;

            Assert.IsAssignableFrom<IEnumerable<NavLocation>>(viewModel);

            NavLocation[] navLocations = ((IEnumerable<NavLocation>)view.ViewData.Model).ToArray();

            Assert.Equal(3, navLocations.Length);

            Assert.True(navLocations[0].IsActive);
            Assert.False(navLocations[1].IsActive);
            Assert.False(navLocations[2].IsActive);
        }
    }
}