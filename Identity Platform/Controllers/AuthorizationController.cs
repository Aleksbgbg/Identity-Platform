namespace Identity.Platform.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Identity.Platform.Attributes.Filters;
    using Identity.Platform.Extensions;
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

        public IActionResult GoogleLogin(string returnUrl)
        {
            string redirectUrl = Url.Action(nameof(GoogleResponse),
                                            new
                                            {
                                                ReturnUrl = returnUrl
                                            });

            return new ChallengeResult("Google",
                                       _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl));
        }

        public async Task<IActionResult> GoogleResponse(string returnUrl)
        {
            ExternalLoginInfo externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();

            if (externalLoginInfo == null)
            {
                return RedirectToAction(nameof(Login));
            }

            SignInResult signInResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, false);

            if (signInResult.Succeeded)
            {
                return Redirect(returnUrl);
            }

            AppUser appUser = new AppUser
            {
                Email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email),
                UserName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Name)
            };

            IdentityResult userCreationResult = await _userManager.CreateAsync(appUser);

            if (!userCreationResult.Succeeded)
            {
                return Forbid();
            }

            IdentityResult addLoginResult = await _userManager.AddLoginAsync(appUser, externalLoginInfo);

            if (!addLoginResult.Succeeded)
            {
                return Forbid();
            }

            IdentityResult addToUserRoleResult = await _userManager.AddToRoleAsync(appUser, "User");

            if (!addToUserRoleResult.Succeeded)
            {
                return Forbid();
            }

            await _signInManager.SignInAsync(appUser, false);
            return Redirect(returnUrl);

        }

        public IActionResult Login(string returnUrl)
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
        [TypeFilter(typeof(EnsureReturnUrlExistsFilter))]
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

        public IActionResult SignUp(string returnUrl)
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
        [TypeFilter(typeof(EnsureReturnUrlExistsFilter))]
        public async Task<IActionResult> SignUp(SignUpCredentials signUpCredentials, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser newUser = new AppUser
                {
                    UserName = signUpCredentials.Username
                };

                IdentityResult createResult = await _userManager.CreateAsync(newUser, signUpCredentials.Password);

                if (createResult.Succeeded)
                {
                    IdentityResult addToRoleResult = await _userManager.AddToRoleAsync(newUser, "User");

                    if (addToRoleResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(newUser, false);
                        return Redirect(returnUrl);
                    }

                    ModelState.AddIdentityErrors(addToRoleResult);
                }

                ModelState.AddIdentityErrors(createResult);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [Authorize]
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