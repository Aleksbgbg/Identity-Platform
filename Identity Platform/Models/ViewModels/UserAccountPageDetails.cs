namespace Identity.Platform.Models.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    public class UserAccountPageDetails
    {
        private const int CommentsPerPage = 5;

        public UserAccountPageDetails(AppUser user, IEnumerable<string> roles, IQueryable<Comment> comments, int currentCommentsPage)
        {
            IQueryable<Comment> userComments = comments.Where(comment => comment.OwnerId == user.Id);

            User = user;
            Roles = roles;
            CommentsPagingInfo = new PagingInfo(currentCommentsPage, userComments.Count(), CommentsPerPage);

            Comments = userComments.AsEnumerable() // No Reverse implementation for IQueryable from db provider
                                   .Reverse()
                                   .Skip(CommentsPagingInfo.ItemsPerPage * (CommentsPagingInfo.CurrentPage - 1))
                                   .Take(CommentsPagingInfo.ItemsPerPage)
                                   .ToArray();
        }

        public AppUser User { get; }

        public IEnumerable<string> Roles { get; }

        public Comment[] Comments { get; }

        public PagingInfo CommentsPagingInfo { get; }
    }
}