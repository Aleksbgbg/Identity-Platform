namespace Identity.Platform.Middleware
{
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public class BackupUserImageMiddleware
    {
        private static readonly Regex UserImageRegex = new Regex(@"\/images\/user\/(?<UserId>[a-z0-9]{8}(?:-[a-z0-9]{4}){3}-[a-z0-9]{12})\.[a-z]*$");

        private readonly RequestDelegate _next;

        public BackupUserImageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await _next(httpContext);

            Match userImagePathMatch = UserImageRegex.Match(httpContext.Request.Path);

            if (userImagePathMatch.Success && httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                httpContext.Response.Redirect($"https://api.adorable.io/avatars/250/{userImagePathMatch.Groups["UserId"].Value}.png");
            }
        }
    }
}