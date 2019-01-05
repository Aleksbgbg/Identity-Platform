namespace Identity.Platform.Controllers
{
    using System.Threading.Tasks;

    using Identity.Platform.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

    [AllowAnonymous]
    public class AuthorizationController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;

        public AuthorizationController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login(string returnUrl = "/")
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser targetUser = await _userManager.FindByNameAsync(login.Username);

            if (targetUser == null)
            {
                ModelState.AddModelError(nameof(login.Username), "Username does not exist.");
                return View();
            }

            SignInResult signInResult = await _signInManager.PasswordSignInAsync
            (
                targetUser,
                login.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if (signInResult.Succeeded)
            {
                return Redirect(returnUrl ?? "/");
            }

            ModelState.AddModelError(nameof(login.Password), "Incorrect password.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }

            return Redirect("/");
        }
    }
}