namespace Identity.Platform.Models.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface ICommentRepository
    {
        IQueryable<Comment> Comments { get; }

        Task AddCommentAsync(Comment comment);

        Task<Comment> RetrieveCommentAsync(string commentId);

        Task DeleteCommentAsync(Comment comment);
    }
}