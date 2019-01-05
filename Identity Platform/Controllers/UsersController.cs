namespace Identity.Platform.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "User, Admin")]
    public class UsersController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }
    }
}