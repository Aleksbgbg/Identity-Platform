namespace Identity.Platform.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View((object)User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }

            return View();
        }
    }
}