namespace Identity.Platform.Attributes.Filters
{
    using System.Collections.Generic;

    using Identity.Platform.Controllers;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.Routing;

    public class EnsureReturnUrlExistsFilter : ActionFilterAttribute
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public EnsureReturnUrlExistsFilter(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            IDictionary<string, object> actionArguments = context.ActionArguments;

            if (!actionArguments.ContainsKey("ReturnUrl") || string.IsNullOrWhiteSpace((string)actionArguments["ReturnUrl"]))
            {
                actionArguments["ReturnUrl"] = _urlHelperFactory.GetUrlHelper(context)
                                                                .Action(nameof(HomeController.Index), "Home");
            }
        }
    }
}