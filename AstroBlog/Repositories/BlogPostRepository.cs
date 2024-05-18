using AstroBlog.Data;
using AstroBlog.Models.Domain;
using Microsoft.AspNetCore.Components.Web;
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

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
           var existingBlog =  await astroBlogDbContext.BlogPosts.FindAsync(id);
            if(existingBlog != null)
            {
                astroBlogDbContext.BlogPosts.Remove(existingBlog);
                await astroBlogDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
           return await astroBlogDbContext.BlogPosts.Include(x=>x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
           return await astroBlogDbContext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlog = await astroBlogDbContext.BlogPosts.Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlog != null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.Content = blogPost.Content;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.Author = blogPost.Author;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.Tags = blogPost.Tags;

                await astroBlogDbContext.SaveChangesAsync();
                return existingBlog;
            }

            return null;
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlhandle)
        {
            return await astroBlogDbContext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x => x.UrlHandle == urlhandle);
        }

       
        public async Task<List<BlogPost>> GetBlogsByTagIdAsync(Guid id)
        {
            return await astroBlogDbContext.BlogPosts
                .Include(x => x.Tags)
                .Where(x => x.Tags.Any(tag => tag.Id == id))
                .ToListAsync();
        }

        public async Task<ICollection<BlogPost>> GetAllMyblogsAsync(Guid userid)
        {
            return await astroBlogDbContext.BlogPosts.Include(x=>x.Tags).Include(x => x.Tags).Where(x => x.OwnerId == userid).ToListAsync();

        }

        //public async Task<IEnumerable<BlogPost>> GetAllMyblogsAsync(Guid userid)
        //{
        //    return await astroBlogDbContext.BlogPosts.Where(x=>x.)Include(x => x.Tags).ToListAsync();
        //}
    }
}
