using AstroBlog.Data;
using AstroBlog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace AstroBlog.Repositories
{
    public class UserRepository : IUserRepository

    {
        private readonly AstroBlogDbContext astroBlogDbContext;

        public UserRepository(AstroBlogDbContext astroBlogDbContext)
        {
            this.astroBlogDbContext = astroBlogDbContext;
        }
        public async Task<Person> AddAsync(Person person)
        {
            await astroBlogDbContext.AddAsync(person);
            await astroBlogDbContext.SaveChangesAsync();
            return person;
        }

        public Task<IEnumerable<Person>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Person?> GetAsync(Guid id)
        {
            return await astroBlogDbContext.People.Include(x => x.BlogPosts).Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Person?> UpdateAsync(Person person)
        {
	        var existingPerson = await astroBlogDbContext.People.Include(x => x.Tags).Include(x => x.BlogPosts)
				.FirstOrDefaultAsync(x => x.Id == person.Id);

	        if (person != null)
	        {
		        person.Id = person.Id;
		        person.BlogPosts = person.BlogPosts;
		        person.Tags = person.Tags;
                person.Email = person.Email;
                person.UserName = person.UserName;
		        await astroBlogDbContext.SaveChangesAsync();
		        return existingPerson;
	        }

	        return null;
        }
	}
}
