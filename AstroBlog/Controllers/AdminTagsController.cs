using AstroBlog.Data;
using AstroBlog.Models.Domain;
using AstroBlog.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace AstroBlog.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly AstroBlogDbContext astroBlogDbContext;
        public AdminTagsController(AstroBlogDbContext astroBlogDbContext) 
        {
            this.astroBlogDbContext = astroBlogDbContext;
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Add")]
        public IActionResult Add(AddTagRequest addTagrequest)
        {
            //maping addrequest to tag domain model
            var tag = new Tag
            {
                Name = addTagrequest.Name,
                DisplayName = addTagrequest.DisplayName,
            };
            astroBlogDbContext.Tags.Add(tag);
            astroBlogDbContext.SaveChanges();
            return View("Add");
        }

        [HttpGet]
        public IActionResult List() 
        {
            // use dbcontet to view list
            var tags = astroBlogDbContext.Tags.ToList();
            return View(tags);      
        }

        [HttpGet]
        public IActionResult Edit(Guid id) 
        {
            var tag = astroBlogDbContext.Tags.FirstOrDefault(t => t.Id == id);
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
        public IActionResult Edit(EditTagRequest editTagrequest) 
        {
            var tag = new Tag {Id= editTagrequest.Id, Name = editTagrequest.Name, DisplayName = editTagrequest.DisplayName };
            var existingTag = astroBlogDbContext.Tags.Find(tag.Id);
            if(existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;
                astroBlogDbContext.SaveChanges();
                //show success 
                return RedirectToAction("Edit", new { id = editTagrequest.Id });

            }
            //show error
            return RedirectToAction("Edit", new {id = editTagrequest.Id });
        }
        [HttpPost]
        public IActionResult Delete(EditTagRequest editTagrequest) 
        {
            var tag = astroBlogDbContext.Tags.Find(editTagrequest.Id);
            if (tag != null) 
            {
                astroBlogDbContext.Tags.Remove(tag);
                astroBlogDbContext.SaveChanges();
                return RedirectToAction("List");

            }
            return RedirectToAction("Edit", new {id = editTagrequest.Id});
        }
    }
}
