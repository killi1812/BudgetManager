using Data.Enums;
using Moq;

namespace Tests.SavingsTests;

public class SavingsControllerTests
{
    private readonly DbContextOptions<BudgetManagerContext> _options;
    private readonly IMapper _mapper;

    public SavingsControllerTests()
    {
        _options = new DbContextOptionsBuilder<BudgetManagerContext>()
            .UseInMemoryDatabase(databaseName: "SavingsTestDatabase")
            .Options;
        _mapper = TestSetupHelpers.SetupAutomapper();
    }

    private SavingsController CreateController(BudgetManagerContext context)
    {
        var savingsService = new SavingService(context);
        var userService = new UserServices(_mapper, context);
        var controller = new SavingsController(savingsService, _mapper, userService);
        controller.MockLoginUser();
        return controller;
    }

    [Fact]
    public async Task Savings_ShouldReturnViewWithSavings()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act
        var result = await controller.Savings();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.IsAssignableFrom<List<SavingsVM>>(viewResult.ViewData.Model);
    }

    [Fact]
    public async Task CreateSaving_ShouldReturnView()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act
        var result = controller.CreateSaving();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task CreateSavingsAction_ShouldRedirectToSavings()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        var user = new User
        {
            Guid = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "User",
            Email = "testuser@test.com",
            PassHash = "hashedpassword",
            Jmbag = "123456789"
        };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        controller.MockLoginUser(user.Guid);

        var newSaving = new SavingsVM
        {
            Goal = 5000,
            Current = 1000,
            Date = DateTime.Now
        };

        // Act
        var result = await controller.CreateSavingsAction(newSaving);

        // Assert
        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.Equal("Savings", redirectResult.Url);

        // Clean
        context.Remove(context.Savings.First());
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task EditSaving_ShouldReturnView()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        var newSaving = new Saving
        {
            Goal = 5000,
            Current = 1000,
            Guid = Guid.NewGuid(),
            Date = DateTime.Now
        };
        await context.Savings.AddAsync(newSaving);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.EditSaving(newSaving.Guid.ToString());

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var vm = Assert.IsType<SavingsVM>(viewResult.ViewData.Model);
        Assert.Equal(newSaving.Guid.ToString(), vm.Guid);

        // Clean
        context.Remove(context.Savings.First());
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task EditSavingsAction_ShouldRedirectToSavings()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        var user = new User
        {
            Guid = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "User",
            Email = "testuser@test.com",
            PassHash = "hashedpassword",
            Jmbag = "123456789"
        };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        controller.MockLoginUser(user.Guid);

        var saving = new Saving
        {
            Goal = 5000,
            Current = 1000,
            Guid = Guid.NewGuid(),
            Date = DateTime.Now,
            UserId = user.Iduser
        };
        await context.Savings.AddAsync(saving);
        await context.SaveChangesAsync();

        var updatedSaving = new SavingsVM
        {
            Guid = saving.Guid.ToString(),
            Goal = 7000,
            Current = 1500
        };

        // Act
        var result = await controller.EditSavingsAction(updatedSaving);

        // Assert
        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.Equal("Savings", redirectResult.Url);

        // Clean
        context.Remove(context.Savings.First());
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteSavingsAction_ShouldRedirectToSavings()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        var saving = new Saving
        {
            Goal = 5000,
            Current = 1000,
            Guid = Guid.NewGuid(),
            Date = DateTime.Now
        };
        await context.Savings.AddAsync(saving);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.DeleteSavingsAction(saving.Guid.ToString());

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Savings", redirectResult.ActionName);
    }

    [Fact]
    public async Task DetailsSaving_ShouldReturnView()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        var saving = new Saving
        {
            Goal = 5000,
            Current = 1000,
            Guid = Guid.NewGuid(),
            Date = DateTime.Now
        };
        await context.Savings.AddAsync(saving);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.DetailsSaving(saving.Guid.ToString());

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var vm = Assert.IsType<SavingsVM>(viewResult.ViewData.Model);
        Assert.Equal(saving.Guid.ToString(), vm.Guid);
    }

    [Fact]
    public async Task Savings_ShouldThrowBadRequestWhenUserNotAuthenticated()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        // Act
        var result = await controller.Savings();

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task EditSaving_ShouldThrowNotFoundWhenSavingDoesNotExist()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await controller.EditSaving(Guid.NewGuid().ToString());
        });
    }

    [Fact]
    public async Task CreateSavingsAction_ShouldThrowBadRequestWhenUserNotFound()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        var newSaving = new SavingsVM
        {
            Goal = 5000,
            Current = 1000,
            Date = DateTime.Now
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await controller.CreateSavingsAction(newSaving);
        });
    }

    [Fact]
    public async Task EditSavingsAction_ShouldThrowBadRequestWhenUserNotFound()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        var updatedSaving = new SavingsVM
        {
            Guid = Guid.NewGuid().ToString(),
            Goal = 5000,
            Current = 1500
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await controller.EditSavingsAction(updatedSaving);
        });
    }

    [Fact]
    public async Task DeleteSavingsAction_ShouldThrowNotFoundWhenSavingDoesNotExist()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await controller.DeleteSavingsAction(Guid.NewGuid().ToString());
        });
    }

    [Fact]
    public async Task DetailsSaving_ShouldThrowNotFoundWhenSavingDoesNotExist()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await controller.DetailsSaving(Guid.NewGuid().ToString());
        });
    }

    [Fact]
    public async Task CreateSavingsAction_ShouldHandleDatabaseFailureGracefully()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        var newSaving = new SavingsVM
        {
            Goal = 5000,
            Current = 1000,
            Date = DateTime.Now
        };

        await context.DisposeAsync(); // Simulate database failure

        // Act & Assert
        await Assert.ThrowsAsync<ObjectDisposedException>(async () =>
        {
            await controller.CreateSavingsAction(newSaving);
        });
    }

    [Fact]
    public async Task Savings_ShouldReturnEmptyListWhenNoSavingsExist()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act
        var result = await controller.Savings();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<SavingsVM>>(viewResult.ViewData.Model);
        Assert.Empty(model);
    }

    [Fact]
    public async Task Savings_ShouldOnlyReturnSavingsForAuthenticatedUser()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        var user1 = new User
        {
            Guid = Guid.NewGuid(),
            FirstName = "User1",
            LastName = "Test",
            Email = "user1@test.com",
            PassHash = "hashedpassword1",
            Jmbag = "123456789"
        };
        var user2 = new User
        {
            Guid = Guid.NewGuid(),
            FirstName = "User2",
            LastName = "Test",
            Email = "user2@test.com",
            PassHash = "hashedpassword2",
            Jmbag = "987654321"
        };

        await context.Users.AddRangeAsync(user1, user2);
        await context.SaveChangesAsync();

        controller.MockLoginUser(user1.Guid);

        var saving1 = new Saving
        {
            Guid = Guid.NewGuid(),
            Goal = 5000,
            Current = 1000,
            UserId = user1.Iduser
        };
        var saving2 = new Saving
        {
            Guid = Guid.NewGuid(),
            Goal = 7000,
            Current = 2000,
            UserId = user2.Iduser
        };

        await context.Savings.AddRangeAsync(saving1, saving2);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.Savings();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<SavingsVM>>(viewResult.ViewData.Model);
        Assert.Single(model);
        Assert.Equal(saving1.Guid.ToString(), model.First().Guid);
    }

    [Fact]
    public async Task DeleteSavingsAction_ShouldRemoveSavingFromDatabase()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        var saving = new Saving
        {
            Guid = Guid.NewGuid(),
            Goal = 5000,
            Current = 1000,
            Date = DateTime.Now
        };
        await context.Savings.AddAsync(saving);
        await context.SaveChangesAsync();

        // Act
        await controller.DeleteSavingsAction(saving.Guid.ToString());

        var result = await context.Savings.FirstOrDefaultAsync(s => s.Guid == saving.Guid);

        // Assert
        Assert.Null(result);
    }

}
