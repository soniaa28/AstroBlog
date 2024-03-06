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
    }
}
