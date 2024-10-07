using BookstoreReactCoreTests.Base;
using BookstoreReactCoreTests.Builder;
using Services;

namespace BookstoreReactCoreTests.Application;

public class BookServiceTests : TestBase
{
    [Fact]
    public async Task GetAll_ShouldReturnAllBooksFromDatabase()
    {
        // Arrange
        await using var context = CreateDbContext();
        var productService = new BookService(context);

        var book1 = BookBuilder.BuildBook();
        context.Books.Add(book1);

        var book2 = BookBuilder.BuildBook();
        context.Books.Add(book2);

        var book3 = BookBuilder.BuildBook();
        context.Books.Add(book3);

        await context.SaveChangesAsync();

        // Act
        var allBooks = await productService.GetAll();

        // Assert
        Assert.NotNull(allBooks);
        Assert.Equal(3, allBooks.Count);
    }

    [Fact]
    public async Task GetWithPagedSearchShouldReturnPagedBooksFromDatabase()
    {
        // Arrange
        await using var context = CreateDbContext();
        var productService = new BookService(context);

        var book1 = BookBuilder.BuildBook();
        context.Books.Add(book1);

        var book2 = BookBuilder.BuildBook();
        context.Books.Add(book2);

        var book3 = BookBuilder.BuildBook();
        context.Books.Add(book3);

        await context.SaveChangesAsync();

        // Act
        var allBooks = await productService.GetWithPagedSearch(null, "ASC", 50, 1);

        // Assert
        Assert.NotNull(allBooks);
        Assert.Equal(3, allBooks.List.Count);
    }

    [Fact]
    public async Task GetById_ShouldReturnBookByIdFromDatabase()
    {
        // Arrange
        await using var context = CreateDbContext();
        var productService = new BookService(context);

        var book = BookBuilder.BuildBook();
        context.Books.Add(book);
        
        await context.SaveChangesAsync();

        // Act
        var bookResult = await productService.GetById(book.Id);

        // Assert
        Assert.NotNull(bookResult);
        Assert.Equal(book.Id, bookResult.Id);
    }

    [Fact]
    public async Task Create_ShouldAddBookToDatabase()
    {
        // Arrange
        await using var context = CreateDbContext();
        var productService = new BookService(context);
        var book = BookBuilder.BuildBook();

        // Act
        await productService.Create(book);

        // Assert
        var savedBook = context.Books.FirstOrDefault(p => p.Id == book.Id);
        Assert.NotNull(savedBook);
        Assert.Equal(book.Title, savedBook.Title);
        Assert.Equal(book.Price, savedBook.Price);
    }

    [Fact]
    public async Task Update_ShouldUpdateBookToDatabase()
    {
        // Arrange
        await using var context = CreateDbContext();
        var productService = new BookService(context);

        var book = BookBuilder.BuildBook();
        context.Books.Add(book);

        await context.SaveChangesAsync();

        // Act
        var titleChanged = book.Title + " CHANGED";
        book.Title = titleChanged;
        var isUpdated = await productService.Update(book.Id, book);
        var bookResult = await productService.GetById(book.Id);

        // Assert
        Assert.True(isUpdated);
        Assert.Equal(bookResult.Title, book.Title);
    }

    [Fact]
    public async Task Delete_ShouldDeleteBookToDatabase()
    {
        // Arrange
        await using var context = CreateDbContext();
        var productService = new BookService(context);

        var book = BookBuilder.BuildBook();
        context.Books.Add(book);

        await context.SaveChangesAsync();

        // Act
        await productService.Delete(book.Id);
        var deletedBook = context.Books.FirstOrDefault(p => p.Id == book.Id);

        // Assert
        Assert.Null(deletedBook);
    }
}


