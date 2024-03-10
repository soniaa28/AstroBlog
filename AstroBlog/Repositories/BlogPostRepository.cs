using AstroBlog.Data;
using AstroBlog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace AstroBlog.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly AstroBlogDbContext astroBlogDbContext;

        public BlogPostRepository(AstroBlogDbContext astroBlogDbContext)
        {
            this.astroBlogDbContext = astroBlogDbContext;
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
           await  astroBlogDbContext.AddAsync(blogPost);
           await astroBlogDbContext.SaveChangesAsync();
           return blogPost;
        }

        public Task<BlogPost?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
           return await astroBlogDbContext.BlogPosts.Include(x=>x.Tags).ToListAsync();
        }

        public Task<BlogPost?> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            throw new NotImplementedException();
        }
    }
}
