namespace Identity.Platform.Models.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    public class UserAccountPageDetails
    {
        private const int CommentsPerPage = 5;

        public UserAccountPageDetails(AppUser user, IEnumerable<string> roles, IQueryable<Comment> comments, int currentCommentsPage)
        {
            User = user;
            Roles = roles;
            Comments = comments;
            CommentsPagingInfo = new PagingInfo(currentCommentsPage, comments.Count(comment => comment.OwnerId == user.Id), CommentsPerPage);
        }

        public AppUser User { get; }

        public IEnumerable<string> Roles { get; }

        public IQueryable<Comment> Comments { get; }

        public PagingInfo CommentsPagingInfo { get; }
    }
}