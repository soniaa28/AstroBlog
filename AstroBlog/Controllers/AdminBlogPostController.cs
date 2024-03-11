using AstroBlog.Models.ViewModel;
using AstroBlog.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc.Rendering;
using AstroBlog.Models.Domain;

namespace AstroBlog.Controllers
{
    public class AdminBlogPostController : Controller
    {
        private readonly ITagRepository tagRepository;
        public IBlogPostRepository blogPostRepository;

        public AdminBlogPostController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }

       

        [HttpGet]
       public async Task <IActionResult> Add() 
        {
            //get tags from repository
           var tags =  await tagRepository.GetAllAsync();
            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            //map view model to domain model

            var blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible,
               
            };
            var selectedTags = new List<Tag>();
            //map tags from selected tags 
            foreach(var selectedTagId in addBlogPostRequest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);
                if (existingTag != null) 
                {
                    selectedTags.Add(existingTag);
                }
            }
            //mapping tags back to domain model
            blogPost.Tags = selectedTags;
            await blogPostRepository.AddAsync(blogPost);
            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            //call the repo
            var blogposts = await blogPostRepository.GetAllAsync();

            return View(blogposts);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //Retrieve the result from the repo
           var blogPost = await blogPostRepository.GetAsync(id);
            var tags = await tagRepository.GetAllAsync();

            if (blogPost != null) 
            {
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    Visible = blogPost.Visible,
                    Tags = tags.Select(t => new SelectListItem
                    {
                        Text = t.Name,
                        Value = t.Id.ToString()
                    }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
                };
                return View(model);
            }
           
            //pass data to view
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            var blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                ShortDescription = editBlogPostRequest.ShortDescription,
                PublishedDate = editBlogPostRequest.PublishedDate,
                Visible = editBlogPostRequest.Visible
            };
            var selectesTags = new List<Tag>();
            foreach(var selectedTag in editBlogPostRequest.SelectedTags)
            {
                if(Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);
                    if (foundTag != null)
                    {
                        selectesTags.Add(foundTag);
                    }
                }
            }
            blogPostDomainModel.Tags = selectesTags;

           var updatedBlog =  await blogPostRepository.UpdateAsync(blogPostDomainModel);
           if(updatedBlog != null)
            {
                //show success 
                return RedirectToAction("Edit");
            }
            return RedirectToAction("Edit");

        }
        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            var deletedBlogPost = await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);
            if(deletedBlogPost != null)
            {
                return RedirectToAction("List");
            }
            //show error
            return RedirectToAction("Edit", new {id = editBlogPostRequest.Id});
        }
    }
}
