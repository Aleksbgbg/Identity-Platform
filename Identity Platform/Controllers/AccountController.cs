namespace Identity.Platform.Controllers
{
    using System;
    using System.IO;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Identity.Platform.Attributes.Filters;
    using Identity.Platform.Extensions;
    using Identity.Platform.Models;
    using Identity.Platform.Models.Repositories;
    using Identity.Platform.Models.ViewModels;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
    using IOFile = System.IO.File;

    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;

        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly ICommentRepository _commentRepository;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IHostingEnvironment hostingEnvironment, ICommentRepository commentRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hostingEnvironment = hostingEnvironment;
            _commentRepository = commentRepository;
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

        [EnsureAnonymous]
        public ViewResult Login(string returnUrl)
        {
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
                UserId = targetUser.Id,
                ImageExtension = targetUser.ImageExtension,
                Username = targetUser.UserName,
                Email = targetUser.Email,
                PhoneNumber = targetUser.PhoneNumber
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
                IFormFile image = userCredentials.Image;

                if (image != null)
                {
                    string userImagesPath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "user");

                    string newExtension = Path.GetExtension(image.FileName);

                    string newExtensionPure = newExtension.TrimStart('.');

                    if (newExtensionPure != targetUser.ImageExtension)
                    {
                        Directory.CreateDirectory(userImagesPath);
                        string filePath = Path.Combine(userImagesPath, string.Concat(targetUser.Id, ".", targetUser.ImageExtension));
                        if (IOFile.Exists(filePath))
                        {
                            IOFile.Delete(filePath);
                        }
                        targetUser.ImageExtension = newExtensionPure;
                    }

                    string newImagePath = Path.Combine(userImagesPath, string.Concat(targetUser.Id, newExtension));

                    using (FileStream fileStream = IOFile.Create(newImagePath))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                }

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

        [Authorize]
        [ActionName("View")]
        public async Task<ViewResult> ViewProfile(int pageNumber = 1)
        {
            AppUser currentUser = await _userManager.GetUserAsync(User);

            ViewBag.AuthenticatedUserId = currentUser.Id;
            ViewBag.ProfileId = currentUser.Id;

            return View(new UserAccountPageDetails(currentUser,
                                                   await _userManager.GetRolesAsync(currentUser),
                                                   _commentRepository.Comments,
                                                   pageNumber));
        }

        [Authorize]
        [ActionName("View")]
        public async Task<IActionResult> ViewProfile(string userId, int pageNumber = 1)
        {
            string currentUserId = User.FindId();

            if (currentUserId == userId)
            {
                return RedirectToAction("View", new
                {
                    // Must nullify UserId route parameter, otherwise
                    // we redirect recursively to the current action
                    UserId = (string)null
                });
            }

            AppUser targetUser = await _userManager.FindByIdAsync(userId);

            if (targetUser == null)
            {
                return NotFound();
            }

            ViewBag.AuthenticatedUserId = currentUserId;
            ViewBag.ProfileId = userId;

            return View(new UserAccountPageDetails(targetUser,
                                                   await _userManager.GetRolesAsync(targetUser),
                                                   _commentRepository.Comments,
                                                   pageNumber));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comment(string userId, Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return View("View");
            }

            AppUser targetUser = await _userManager.FindByIdAsync(userId);

            if (targetUser == null)
            {
                return BadRequest();
            }

            comment.OwnerId = userId;
            comment.AuthorId = User.FindId();
            comment.PostedAt = DateTime.Now;

            await _commentRepository.AddCommentAsync(comment);

            return RedirectToAction("View",
                                    new
                                    {
                                        UserId = userId
                                    });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(string userId, string commentId)
        {
            string authenticatedUserId = User.FindId();
            Comment comment = await _commentRepository.RetrieveCommentAsync(commentId);

            if (authenticatedUserId == comment.AuthorId || authenticatedUserId == comment.OwnerId)
            {
                await _commentRepository.DeleteCommentAsync(comment);

                return RedirectToAction("View",
                                        new
                                        {
                                            UserId = userId
                                        });
            }

            return Forbid();
        }
    }
}