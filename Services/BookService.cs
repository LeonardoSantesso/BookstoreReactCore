using Services.Interfaces;
using Models;
using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class BookService : IBookService
    {
        private readonly BookstoreReactCoreContext _context;

        public BookService(BookstoreReactCoreContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAll()
        {
            return await _context.Books.OrderBy(i => i.Title).ToListAsync();
        }

        public async Task<List<Book>> GetWithPagedSearch(string title, string sortDirection, int pageSize, int page)
        {
            //TODO: Need to implement
            return await _context.Books.OrderBy(i => i.Title).ToListAsync();
        }

        public async Task<Book> GetById(long id)
        {
            return await _context.Books.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Book> Create(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<bool> Update(long id, Book book)
        {
            var bookDb = await _context.Books.FirstOrDefaultAsync(i => i.Id == id);

            if (bookDb == null)
                return false;

            bookDb.Title = book.Title;
            bookDb.Author = book.Author;
            bookDb.LaunchDate = book.LaunchDate;
            bookDb.Price = book.Price;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(long id)
        {
            var bookDb = await _context.Books.FirstOrDefaultAsync(i => i.Id == id);

            if (bookDb == null)
                return false;

            _context.Books.Remove(bookDb);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
