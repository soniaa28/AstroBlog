using AstroBlog.Models.Domain;
using AstroBlog.Models.ViewModel;
using AstroBlog.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AstroBlog.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;
        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagrequest)
        {
            //maping addrequest to tag domain model
            var tag = new Tag
            {
                Name = addTagrequest.Name,
                DisplayName = addTagrequest.DisplayName,
            };
            await tagRepository.AddAsync(tag);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            // use dbcontet to view list
            var tags = await  tagRepository.GetAllAsync();
            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await tagRepository.GetAsync(id);
            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(editTagRequest);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagrequest)
        {
            var tag = new Tag { Id = editTagrequest.Id, Name = editTagrequest.Name, DisplayName = editTagrequest.DisplayName };
            var updatedTag = await tagRepository.UpdateAsync(tag);

            if (updatedTag != null)
            {
                //show success success notification
            }
            else
            {
                //show error
            }
            //show error
            return RedirectToAction("List", new { id = editTagrequest.Id });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagrequest)
        {
            var deletedTag = await tagRepository.DeleteAsync(editTagrequest.Id);
            if (deletedTag != null)
            {
                //show success notification
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = editTagrequest.Id });
        }
    }
}
