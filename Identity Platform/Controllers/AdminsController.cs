namespace Identity.Platform.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "Admin")]
    public class AdminsController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }
    }
}