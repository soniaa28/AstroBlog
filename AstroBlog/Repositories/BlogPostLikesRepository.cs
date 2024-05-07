
using AstroBlog.Data;
using AstroBlog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace AstroBlog.Repositories
{
    public class BlogPostLikesRepository : IBlogPostLikesRepository
    {
        private readonly AstroBlogDbContext astroBlogDbContext;

        public BlogPostLikesRepository(AstroBlogDbContext astroBlogDbContext)
        {
            this.astroBlogDbContext = astroBlogDbContext;
        }

        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogpostlike)
        {
            await astroBlogDbContext.Likes.AddAsync(blogpostlike);
            await astroBlogDbContext.SaveChangesAsync();
            return blogpostlike;
        }

        public async Task<IEnumerable<BlogPostLike>> GetLikesForBlogForUser(Guid blogPostId)
        {
            return await astroBlogDbContext.Likes.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
           return  await astroBlogDbContext.Likes.CountAsync(x=>x.BlogPostId == blogPostId);
        }
    }
}
