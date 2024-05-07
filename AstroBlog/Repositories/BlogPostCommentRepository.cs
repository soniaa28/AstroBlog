using AstroBlog.Data;
using AstroBlog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace AstroBlog.Repositories
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly AstroBlogDbContext _astroBlogDbContext;

        public BlogPostCommentRepository(AstroBlogDbContext astroBlogDbContext)
        {
            _astroBlogDbContext = astroBlogDbContext;
        }
        public async  Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
            await _astroBlogDbContext.AddAsync(blogPostComment);
            await _astroBlogDbContext.SaveChangesAsync();
            return blogPostComment;
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentsByBlogdAsync(Guid blogPostId)
        {
           return await _astroBlogDbContext.Comments.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }
    }
}
