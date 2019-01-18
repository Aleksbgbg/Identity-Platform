namespace Identity.Platform.Models
{
    using Microsoft.AspNetCore.Identity;

    public class AppUser : IdentityUser
    {
        public string ImageExtension { get; set; }
    }
}