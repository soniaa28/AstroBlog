using AstroBlog.Models.Domain;
using System.Collections;

namespace AstroBlog.Models.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<BlogPost> BlogPosts { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}
