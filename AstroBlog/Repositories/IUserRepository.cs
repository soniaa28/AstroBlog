using AstroBlog.Models.Domain;

namespace AstroBlog.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<Person>> GetAllAsync();
        Task<Person?> GetAsync(Guid id);
        Task<Person> AddAsync(Person person);
        Task<Person?> UpdateAsync(Person person);

    }
}
