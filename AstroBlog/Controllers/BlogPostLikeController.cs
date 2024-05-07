using AstroBlog.Models.Domain;
using AstroBlog.Models.ViewModel;
using AstroBlog.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AstroBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BlogPostLikeController : ControllerBase
    {
        private readonly IBlogPostLikesRepository _blogPostLikesRepository;

        public BlogPostLikeController(IBlogPostLikesRepository blogPostLikesRepository)
        {
            _blogPostLikesRepository = blogPostLikesRepository;
        }
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLike([FromBody] AddLikeRequest addLikeRequest)
        {
            var model = new BlogPostLike()
            {
                BlogPostId = addLikeRequest.BlogPostId,
                UserId = addLikeRequest.UserId
            };
           await _blogPostLikesRepository.AddLikeForBlog(model);
           return Ok();
        }

        [HttpGet]
        [Route("{blogPostId:Guid}/totalLikes")]
        public async Task<IActionResult> GetTotalLikesForBlog([FromRoute] Guid blogPostId)

        {
            var totalLikes = await _blogPostLikesRepository.GetTotalLikes(blogPostId);
            return Ok(totalLikes);
        }
    }
}
