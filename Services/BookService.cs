using Services.Interfaces;
using Models;
using DAL.Context;
using Microsoft.EntityFrameworkCore;
using Models.DTO;

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

        public async Task<PagedSearchDTO<Book>> GetWithPagedSearch(string title, string sortDirection, int pageSize, int page)
        {
            var sort = !string.IsNullOrWhiteSpace(sortDirection) && !sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase) ? "asc" : "desc";
            var size = pageSize < 1 ? 10 : pageSize;
            var offset = page > 0 ? (page - 1) * size : 0;

            IQueryable<Book> query = _context.Books;

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(b => b.Title.Contains(title));
            }

            query = sort == "asc" ? query.OrderBy(b => b.Title) : query.OrderByDescending(b => b.Title);

            int totalResults = await query.CountAsync();

            var books = await query.Skip(offset).Take(size).ToListAsync();

            return new PagedSearchDTO<Book>
            {
                CurrentPage = page,
                List = books,
                PageSize = size,
                SortDirections = sort,
                TotalResults = totalResults
            };
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
