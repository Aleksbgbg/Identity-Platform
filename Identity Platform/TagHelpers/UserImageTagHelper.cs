namespace Identity.Platform.TagHelpers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Identity.Platform.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [HtmlTargetElement("img", Attributes = "user-claims-principal")]
    [HtmlTargetElement("img", Attributes = "user-id")]
    public class UserImageTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        private readonly IActionContextAccessor _actionContextAccessor;

        private readonly UserManager<AppUser> _userManager;

        public UserImageTagHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor, UserManager<AppUser> userManager)
        {
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccessor = actionContextAccessor;
            _userManager = userManager;
        }

        public ClaimsPrincipal UserClaimsPrincipal { get; set; }

        public string UserId { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            AppUser currentUser;

            if (UserClaimsPrincipal == null)
            {
                if (UserId == null)
                {
                    return;
                }

                currentUser = await _userManager.FindByIdAsync(UserId);
            }
            else
            {
                currentUser = await _userManager.GetUserAsync(UserClaimsPrincipal);
            }

            output.Attributes.SetAttribute
            (
                "src",
                _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext)
                                 .Content($"~/images/user/{currentUser.Id}.{currentUser.ImageExtension}")
            );
        }
    }
}