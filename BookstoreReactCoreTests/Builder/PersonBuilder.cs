using Models;

namespace BookstoreReactCoreTests.Builder;

public static class PersonBuilder
{
    public static Person BuildPerson()
    {
        return new Person
        {
            Address = Faker.Address.StreetAddress(true),
            Enabled = true,
            FirstName = Faker.Name.First(),
            LastName = Faker.Name.Last(),
            Gender = "Male"
        };
    }
}
