namespace Identity.Platform.Models.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface ICommentRepository
    {
        IQueryable<Comment> Comments { get; }

        void AddComment(Comment comment);

        Task AddCommentAsync(Comment comment);
    }
}