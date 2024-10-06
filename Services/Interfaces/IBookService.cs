using Models;

namespace Services.Interfaces
{
    public interface IBookService
    {
        Task<List<Book>> GetAll();
        Task<List<Book>> GetWithPagedSearch(string title, string sortDirection, int pageSize, int page);
        Task<Book> GetById(long id);
        Task<Book> Create(Book book);
        Task<bool> Update(long id, Book book);
        Task<bool> Delete(long id);
    }
}
