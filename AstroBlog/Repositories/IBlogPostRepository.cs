using AstroBlog.Models.Domain;

namespace AstroBlog.Repositories
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<BlogPost?> GetAsync(Guid id);
        Task<BlogPost> AddAsync(BlogPost blogPost);
        Task<BlogPost?> UpdateAsync(BlogPost blogPost);
        Task<BlogPost?> DeleteAsync(Guid id);
        Task<BlogPost?> GetByUrlHandleAsync(string urlhandle);
        Task<List<BlogPost>> GetBlogsByTagIdAsync(Guid id);

    }
}
