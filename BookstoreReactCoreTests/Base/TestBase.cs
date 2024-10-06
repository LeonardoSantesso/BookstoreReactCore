using Microsoft.EntityFrameworkCore;
using DAL.Context;

namespace BookstoreReactCoreTests.Base;

public abstract class TestBase : IDisposable
{
    private readonly DbContextOptions<BookstoreReactCoreContext> _options;

    protected TestBase()
    {
        _options = new DbContextOptionsBuilder<BookstoreReactCoreContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }

    protected BookstoreReactCoreContext CreateDbContext()
    {
        var context = new BookstoreReactCoreContext(_options);

        context.Database.EnsureCreated();
        return context;
    }

    public void Dispose()
    {
        using var context = new BookstoreReactCoreContext(_options);
        context.Database.EnsureDeleted();
    }
}

