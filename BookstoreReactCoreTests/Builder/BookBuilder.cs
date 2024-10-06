using Models;

namespace BookstoreReactCoreTests.Builder;

public static class BookBuilder
{
    public static Book BuildBook()
    {
        return new Book
        {
            Author = Faker.Name.FullName(),
            LaunchDate = DateTime.Now,
            Price = 100,
            Title = $"Book title test {Faker.RandomNumber.Next()}"
        };
    }
}

