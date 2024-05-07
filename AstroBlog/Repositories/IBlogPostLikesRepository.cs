using AstroBlog.Models.Domain;

namespace AstroBlog.Repositories
{
    public interface IBlogPostLikesRepository
    {
      Task<int>  GetTotalLikes(Guid blogPostId);
      Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogpostlike);
      Task<IEnumerable<BlogPostLike>> GetLikesForBlogForUser(Guid blogPostId);
    }
}
