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

        public IQueryable<Comment> Comments { get; }

        public void AddComment(Comment comment)
        {
            _appIdentityDbContext.Comments.Add(comment);
            _appIdentityDbContext.SaveChanges();
        }

        public async Task AddCommentAsync(Comment comment)
        {
            await _appIdentityDbContext.Comments.AddAsync(comment);
            await _appIdentityDbContext.SaveChangesAsync();
        }
    }
}