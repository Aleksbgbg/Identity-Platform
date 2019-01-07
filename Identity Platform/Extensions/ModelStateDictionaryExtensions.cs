namespace Identity.Platform.Extensions
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    internal static class ModelStateDictionaryExtensions
    {
        internal static void AddIdentityErrors(this ModelStateDictionary modelState, IdentityResult identityResult)
        {
            modelState.AddIdentityErrors(identityResult.Errors);
        }

        internal static void AddIdentityErrors(this ModelStateDictionary modelState, IEnumerable<IdentityError> errors)
        {
            foreach (IdentityError error in errors)
            {
                modelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}