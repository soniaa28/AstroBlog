using AstroBlog.Models.Domain;

namespace AstroBlog.Repositories
{
    public interface IBlogPostCommentRepository
    {
        Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment);
        Task<IEnumerable<BlogPostComment>> GetCommentsByBlogdAsync(Guid blogPostId);
    }
}
