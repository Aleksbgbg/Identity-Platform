namespace Identity.Platform.Models.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    using Identity.Platform.Models.Database;

    public class CommentRepository : ICommentRepository
    {
        private readonly AppIdentityDbContext _appIdentityDbContext;

        public CommentRepository(AppIdentityDbContext appIdentityDbContext)
        {
            _appIdentityDbContext = appIdentityDbContext;
        }

        public IQueryable<Comment> Comments => _appIdentityDbContext.Comments;

        public async Task AddCommentAsync(Comment comment)
        {
            await _appIdentityDbContext.Comments.AddAsync(comment);
            await _appIdentityDbContext.SaveChangesAsync();
        }

        public Task<Comment> RetrieveCommentAsync(string commentId)
        {
            return _appIdentityDbContext.Comments.FindAsync(commentId);
        }

        public async Task DeleteCommentAsync(Comment comment)
        {
            _appIdentityDbContext.Comments.Remove(comment);
            await _appIdentityDbContext.SaveChangesAsync();
        }
    }
}