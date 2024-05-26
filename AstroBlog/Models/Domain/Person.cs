namespace AstroBlog.Models.Domain
{
    public class Person
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
