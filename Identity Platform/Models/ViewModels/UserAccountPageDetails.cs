namespace Identity.Platform.Models.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    public class UserAccountPageDetails
    {
        public UserAccountPageDetails(AppUser user, IEnumerable<string> roles, IQueryable<Comment> comments)
        {
            User = user;
            Roles = roles;
            Comments = comments;
        }

        public AppUser User { get; }

        public IEnumerable<string> Roles { get; }

        public IQueryable<Comment> Comments { get; }
    }
}