using Models;

namespace Services.Interfaces
{
    public interface IPersonService
    {
        Task<List<Person>> GetAll();
        Task<Person> GetById(long id);
        Task<List<Person>> FindByName(string firstName, string lastName);
        Task<Person> Create(Person person);
        Task<bool> Update(long id, Person person);
        Task<bool> Delete(long id);
        Task<bool> Disable(long id);
    }
}
