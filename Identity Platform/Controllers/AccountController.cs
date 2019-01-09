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
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public ViewResult AccessDenied()
        {
            return View();
        }

        public ChallengeResult GoogleLogin(string returnUrl)
        {
            string redirectUrl = Url.Action(nameof(GoogleResponse),
                                            new
                                            {
                                                ReturnUrl = returnUrl
                                            });

            return new ChallengeResult("Google",
                                       _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl));
        }

        [TypeFilter(typeof(EnsureReturnUrlExistsFilter))]
        public async Task<IActionResult> GoogleResponse(string returnUrl)
        {
            ExternalLoginInfo externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();

            if (externalLoginInfo == null)
            {
                return RedirectToAction(nameof(SignIn));
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

        [EnsureAnonymous]
        public ViewResult SignIn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(EnsureReturnUrlExistsFilter))]
        public async Task<IActionResult> SignIn(Login login, string returnUrl)
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

        [EnsureAnonymous]
        public ViewResult SignUp(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [TypeFilter(typeof(EnsureReturnUrlExistsFilter))]
        public async Task<IActionResult> SignUp(UserCredentials userCredentials, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser newUser = new AppUser
                {
                    UserName = userCredentials.Username,
                    Email = userCredentials.Email,
                    PhoneNumber = userCredentials.PhoneNumber
                };

                IdentityResult createResult = await _userManager.CreateAsync(newUser, userCredentials.Password);

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
        public async Task<RedirectToActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Authorize]
        public async Task<ViewResult> Edit()
        {
            AppUser targetUser = await _userManager.GetUserAsync(User);

            return View(new UserCredentials
            {
                Username = targetUser.UserName
            });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserCredentials userCredentials)
        {
            AppUser targetUser = await _userManager.GetUserAsync(User);

            userCredentials.Username = targetUser.UserName;

            if (ModelState.IsValid)
            {
                targetUser.Email = userCredentials.Email;
                targetUser.PhoneNumber = userCredentials.PhoneNumber;

                IdentityResult updateResult = await _userManager.UpdateAsync(targetUser);

                if (updateResult.Succeeded)
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }

                ModelState.AddIdentityErrors(updateResult);
            }

            return View(userCredentials);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<RedirectToActionResult> Delete()
        {
            AppUser targetUser = await _userManager.GetUserAsync(User);

            IdentityResult deleteResult = await _userManager.DeleteAsync(targetUser);

            if (!deleteResult.Succeeded)
            {
                // TODO: Redirect to error page?
                ModelState.AddIdentityErrors(deleteResult);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}