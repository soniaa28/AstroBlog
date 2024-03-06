using AstroBlog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace AstroBlog.Data
{
    public class AstroBlogDbContext : DbContext
    {
        public AstroBlogDbContext(DbContextOptions<AstroBlogDbContext> options) : base(options) { }

        public DbSet<BlogPost> BlogPosts { get; set; }

        public DbSet<Tag> Tags { get; set; }
    }
}
