using Services.Interfaces;
using Models;
using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly BookstoreReactCoreContext _context;

        public PersonService(BookstoreReactCoreContext context)
        {
            _context = context;
        }

        public async Task<List<Person>> GetAll()
        {
            return await _context.Persons.OrderBy(i => i.FirstName).ToListAsync();
        }

        public async Task<Person> GetById(long id)
        {
            return await _context.Persons.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<Person>> FindByName(string firstName, string lastName)
        {
            var query = _context.Persons.OrderBy(i => i.FirstName).AsQueryable();

            if (!string.IsNullOrEmpty(firstName))
                query = query.Where(i => i.FirstName == firstName);

            if (!string.IsNullOrEmpty(lastName))
                query = query.Where(i => i.LastName == lastName);

            return await query.ToListAsync();
        }

        public async Task<Person> Create(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
            return person;
        }

        public async Task<bool> Update(long id, Person person)
        {
            var personDb = await _context.Persons.FirstOrDefaultAsync(i => i.Id == id);

            if (personDb == null)
                return false;

            personDb.FirstName = person.FirstName;
            personDb.LastName = person.LastName;
            personDb.Address = person.Address;
            personDb.Enabled = person.Enabled;
            personDb.Gender = person.Gender;
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(long id)
        {
            var personDb = await _context.Persons.FirstOrDefaultAsync(i => i.Id == id);

            if (personDb == null)
                return false;

            _context.Persons.Remove(personDb);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Disable(long id)
        {
            var personDb = await _context.Persons.FirstOrDefaultAsync(i => i.Id == id);

            if (personDb == null)
                return false;

            personDb.Enabled = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
