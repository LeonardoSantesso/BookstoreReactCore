using BookstoreReactCoreTests.Base;
using BookstoreReactCoreTests.Builder;
using Models;
using Services;

namespace BookstoreReactCoreTests.Application;

public class PersonServiceTests : TestBase
{
    [Fact]
    public async Task GetAll_ShouldReturnAllPersonsFromDatabase()
    {
        // Arrange
        await using var context = CreateDbContext();
        var productService = new PersonService(context);

        var person1 = PersonBuilder.BuildPerson();
        context.Persons.Add(person1);

        var person2 = PersonBuilder.BuildPerson();
        context.Persons.Add(person2);

        var person3 = PersonBuilder.BuildPerson();
        context.Persons.Add(person3);

        await context.SaveChangesAsync();

        // Act
        var allPersons = await productService.GetAll();

        // Assert
        Assert.NotNull(allPersons);
        Assert.Equal(3, allPersons.Count);
    }
    
    [Fact]
    public async Task GetById_ShouldReturnPersonByIdFromDatabase()
    {
        // Arrange
        await using var context = CreateDbContext();
        var productService = new PersonService(context);

        var person = PersonBuilder.BuildPerson();
        context.Persons.Add(person);

        await context.SaveChangesAsync();

        // Act
        var personResult = await productService.GetById(person.Id);

        // Assert
        Assert.NotNull(personResult);
        Assert.Equal(person.Id, personResult.Id);
    }

    [Fact]
    public async Task GetById_ShouldReturnPersonByNameFromDatabase()
    {
        // Arrange
        await using var context = CreateDbContext();
        var productService = new PersonService(context);

        var person = PersonBuilder.BuildPerson();
        context.Persons.Add(person);

        await context.SaveChangesAsync();

        // Act
        var personResult = await productService.FindByName(person.FirstName, person.LastName);

        // Assert
        Assert.NotNull(personResult);
        Assert.Equal(person.Id, personResult.First().Id);
    }

    [Fact]
    public async Task Create_ShouldAddPersonToDatabase()
    {
        // Arrange
        await using var context = CreateDbContext();
        var productService = new PersonService(context);
        var person = PersonBuilder.BuildPerson();

        // Act
        await productService.Create(person);

        // Assert
        var savedPerson = context.Persons.FirstOrDefault(p => p.Id == person.Id);
        Assert.NotNull(savedPerson);
        Assert.Equal(person.FirstName, savedPerson.FirstName);
        Assert.Equal(person.LastName, savedPerson.LastName);
    }

    [Fact]
    public async Task Update_ShouldUpdatePersonToDatabase()
    {
        // Arrange
        await using var context = CreateDbContext();
        var productService = new PersonService(context);

        var person = PersonBuilder.BuildPerson();
        context.Persons.Add(person);

        await context.SaveChangesAsync();

        // Act
        var lastNameChanged = person.LastName + " CHANGED";
        person.LastName = lastNameChanged;
        var isUpdated = await productService.Update(person.Id, person);
        var personResult = await productService.GetById(person.Id);

        // Assert
        Assert.True(isUpdated);
        Assert.Equal(personResult.LastName, person.LastName);
    }

    [Fact]
    public async Task Delete_ShouldDeletePersonToDatabase()
    {
        // Arrange
        await using var context = CreateDbContext();
        var productService = new PersonService(context);

        var person = PersonBuilder.BuildPerson();
        context.Persons.Add(person);

        await context.SaveChangesAsync();

        // Act
        await productService.Delete(person.Id);
        var deletedPerson = context.Persons.FirstOrDefault(p => p.Id == person.Id);

        // Assert
        Assert.Null(deletedPerson);
    }
    
    [Fact]
    public async Task Disable_ShouldDisablePersonToDatabase()
    {
        // Arrange
        await using var context = CreateDbContext();
        var productService = new PersonService(context);

        var person = PersonBuilder.BuildPerson();
        context.Persons.Add(person);

        await context.SaveChangesAsync();

        // Act
        await productService.Disable(person.Id);
        var disabledPerson = context.Persons.FirstOrDefault(p => p.Id == person.Id);

        // Assert
        Assert.NotNull(disabledPerson);
        Assert.False(disabledPerson.Enabled);
    }
}

