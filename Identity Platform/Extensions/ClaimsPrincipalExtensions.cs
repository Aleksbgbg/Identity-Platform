namespace Identity.Platform.Extensions
{
    using System.Security.Claims;

    internal static class ClaimsPrincipalExtensions
    {
        internal static string FindId(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        internal static string FindUsername(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(ClaimTypes.Name);
        }
    }
}