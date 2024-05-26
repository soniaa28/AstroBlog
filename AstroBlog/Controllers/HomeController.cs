using AstroBlog.Models;
using AstroBlog.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AstroBlog.Models.Domain;
using AstroBlog.Models.ViewModel;

namespace AstroBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostLikesRepository _blogPostLikesRepository;


        public HomeController(ILogger<HomeController> logger, IBlogPostRepository blogPostRepository , ITagRepository tagRepository, IBlogPostLikesRepository blogPostLikesRepository)
        {
            _logger = logger;
            this.blogPostRepository = blogPostRepository;
            this.tagRepository = tagRepository;
            _blogPostLikesRepository = blogPostLikesRepository;
        }

        public async Task<IActionResult> Index()
        {
            var blogposts = await blogPostRepository.GetAllAsync(); 
            foreach(var post in blogposts)
            {
                var totalLikes = await _blogPostLikesRepository.GetLikesForBlogForUser(post.Id);
                post.Likes = (ICollection<BlogPostLike>)totalLikes;
            }
            var tags = await tagRepository.GetAllAsync();
            var model = new HomeViewModel() { BlogPosts = blogposts, Tags = tags };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
