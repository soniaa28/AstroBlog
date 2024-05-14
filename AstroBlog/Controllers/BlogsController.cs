using AstroBlog.Models.Domain;
using AstroBlog.Models.ViewModel;
using AstroBlog.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AstroBlog.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostLikesRepository blogPostLikesRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IBlogPostCommentRepository _blogPostComment;

        public BlogsController(IBlogPostRepository blogPostRepository, IBlogPostLikesRepository blogPostLikesRepository , SignInManager<IdentityUser> signInManager,UserManager<IdentityUser> userManager,IBlogPostCommentRepository blogPostComment)
        {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikesRepository = blogPostLikesRepository;
            this.signInManager = signInManager;
            _userManager = userManager;
            _blogPostComment = blogPostComment;
        }
        [HttpGet]
        public async Task< IActionResult> Index(string urlHandle)
        {
            var liked = false;
            var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);
            var blogpostlikeviewmodel = new BlogDetailsViewModel();
            if (blogPost != null)
            {
               var totallikes = await blogPostLikesRepository.GetTotalLikes(blogPost.Id);
               if (signInManager.IsSignedIn(User))
               {
                   var likesForBlog = await blogPostLikesRepository.GetLikesForBlogForUser(blogPost.Id);
                   var userid = _userManager.GetUserId(User);
                   if (userid != null)
                   {
                       var likesfromuser = likesForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userid));
                        liked = likesfromuser != null;
                   }
               }

               var blogComments = await _blogPostComment.GetCommentsByBlogdAsync(blogPost.Id);
               var blogCommentForView = new List<BlogComment>();
                
                    foreach(var blogcomment in blogComments)
                    {
                        blogCommentForView.Add(new BlogComment
                        {
                            Description = blogcomment.Description,
                            DateAdded = blogcomment.DateAdded,
                            Username = (await _userManager.FindByIdAsync(blogcomment.UserId.ToString())).UserName,
                        });
                    }
                
               blogpostlikeviewmodel = new BlogDetailsViewModel()
               {
                   Id = blogPost.Id,
                   Content = blogPost.Content,
                   PageTitle = blogPost.PageTitle,
                   Author = blogPost.Author,
                   FeaturedImageUrl = blogPost.FeaturedImageUrl,
                   Heading = blogPost.Heading,
                   PublishedDate = blogPost.PublishedDate,
                   ShortDescription = blogPost.ShortDescription,
                   UrlHandle = blogPost.UrlHandle,
                   Visible = blogPost.Visible,
                   Tags = blogPost.Tags,
                   TotalLikes = totallikes,
                   Liked = liked,
                   Comments = blogCommentForView

               };
            }

           
            return View(blogpostlikeviewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
        {
            if (signInManager.IsSignedIn(User))
            {
                var domainmodel = new BlogPostComment()
                {
                    BlogPostId = blogDetailsViewModel.Id,
                    Description = blogDetailsViewModel.CommentDescription,
                    UserId = Guid.Parse(_userManager.GetUserId(User)),
                    DateAdded = DateTime.Now
                };
                await _blogPostComment.AddAsync(domainmodel);
                return RedirectToAction("Index", "Blogs", new { urlHandle = blogDetailsViewModel.UrlHandle });
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> BlogByTag(Guid id)
        {
            var blogposts = await blogPostRepository.GetBlogsByTagIdAsync(id);
            foreach (var post in blogposts)
            {
                var totalLikes = await blogPostLikesRepository.GetLikesForBlogForUser(post.Id);
                post.Likes = (ICollection<BlogPostLike>)totalLikes;
            }
            return View(blogposts);
        }
    }
}
