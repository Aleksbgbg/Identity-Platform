namespace Identity.Platform.Models.ViewModels
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;

    public class UserLogin
    {
        public UserLogin(AppUser user, IEnumerable<string> roles, bool isAuthenticatedUser)
        {
            User = user;
            Roles = roles;
            IsAuthenticatedUser = isAuthenticatedUser;
        }

        public AppUser User { get; }

        public IEnumerable<string> Roles { get; }

        public bool IsAuthenticatedUser { get; }
    }
}