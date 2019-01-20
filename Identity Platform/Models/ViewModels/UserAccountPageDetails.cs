namespace Identity.Platform.Models.ViewModels
{
    using System.Collections.Generic;

    public class UserAccountPageDetails
    {
        public UserAccountPageDetails(AppUser user, bool isAuthenticatedUser, IEnumerable<string> roles, IEnumerable<Comment> comments)
        {
            User = user;
            IsAuthenticatedUser = isAuthenticatedUser;
            Roles = roles;
            Comments = comments;
        }

        public AppUser User { get; }

        public bool IsAuthenticatedUser { get; }

        public IEnumerable<string> Roles { get; }

        public IEnumerable<Comment> Comments { get; }
    }
}