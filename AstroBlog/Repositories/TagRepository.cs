using AstroBlog.Data;
using AstroBlog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace AstroBlog.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly AstroBlogDbContext astroBlogDbContext;
        public TagRepository(AstroBlogDbContext astroBlogDbContext)
        {
            this.astroBlogDbContext = astroBlogDbContext;
        }
        public async Task<Tag> AddAsync(Tag tag)
        {
            await astroBlogDbContext.Tags.AddAsync(tag);
            await astroBlogDbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag> DeleteAsync(Guid id)
        {
            var existingTag = await astroBlogDbContext.Tags.FindAsync(id);
            if (existingTag != null)
            {
                astroBlogDbContext.Tags.Remove(existingTag);
                await astroBlogDbContext.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await astroBlogDbContext.Tags.ToListAsync();
        }

        public Task<Tag> GetAsync(Guid id)
        {
            return astroBlogDbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);

        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await astroBlogDbContext.Tags.FindAsync(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;
                await astroBlogDbContext.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }
    }
}
