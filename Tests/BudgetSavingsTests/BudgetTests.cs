namespace Tests.BudgetTests;

public class BudgetControllerTests
{
    private readonly DbContextOptions<BudgetManagerContext> _options;
    private readonly IMapper _mapper;

    public BudgetControllerTests()
    {
        _options = new DbContextOptionsBuilder<BudgetManagerContext>()
            .UseInMemoryDatabase(databaseName: "BudgetTestDatabase")
            .Options;
        _mapper = TestSetupHelpers.SetupAutomapper();
    }

    private BudgetController CreateController(BudgetManagerContext context)
    {
        var budgetService = new BudgetService(context);
        var userService = new UserServices(_mapper, context);
        var categoryService = new CategoryService(context);
        var controller = new BudgetController(budgetService, _mapper, categoryService, userService);
        controller.MockLoginUser();
        return controller;
    }

    [Fact]
    public async Task Budgets_ShouldReturnViewWithBudgets()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act
        var result = await controller.Budgets();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.IsAssignableFrom<List<BudgetVM>>(viewResult.ViewData.Model);
    }

    [Fact]
    public async Task CreateBudget_ShouldReturnView()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act
        var result = controller.CreateBudget();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task CreateBudgetAction_ShouldRedirectToBudgets()
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

        var category = new Category
        {
            Guid = Guid.NewGuid(),
            CategoryName = "Test Category",
            Color = "Blue"
        };
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        var newBudget = new BudgetVM
        {
            CategoryName = "Test Budget",
            Sum = 1000,
            CategoryGuid = category.Guid.ToString()
        };

        // Act
        var result = await controller.CreateBudgetAction(newBudget);

        // Assert
        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.Equal("Budgets", redirectResult.Url);

        // Clean
        context.Remove(context.Budgets.First());
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task EditBudget_ShouldReturnView()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);
        var newBudget = new Budget
        {
            CategoryId = 1,
            Sum = 1000,
            Guid = Guid.NewGuid(),
        };
        await context.AddAsync(newBudget);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.EditBudget(newBudget.Guid.ToString());

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        BudgetVM? vm = viewResult.Model as BudgetVM;
        if (vm == null)
            Assert.Fail("Wrong model");

        Assert.Equal(newBudget.Guid.ToString(), vm.Guid);

        // Clean
        context.Remove(context.Budgets.First());
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task EditBudgetAction_ShouldRedirectToBudgets()
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

        var category = new Category
        {
            Guid = Guid.NewGuid(),
            CategoryName = "Test Category",
            Color = "Blue"
        };
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        var budget = new Budget
        {
            CategoryId = category.Idcategory, 
            Category = category,
            UserId = user.Iduser, 
            User = user,
            Sum = 1000,
            Guid = Guid.NewGuid()
        };
        context.Budgets.Add(budget);
        await context.SaveChangesAsync();

        var updatedBudget = new BudgetVM
        {
            Guid = budget.Guid.ToString(),
            CategoryName = "Updated Budget",
            Sum = 2000,
            CategoryGuid = category.Guid.ToString() 
        };

        // Act
        var result = await controller.EditBudgetAction(updatedBudget);

        // Assert
        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.Equal("Budgets", redirectResult.Url);

        // Clean
        context.Remove(context.Budgets.First());
        await context.SaveChangesAsync();
    }



    [Fact]
    public async Task DeleteBudget_ShouldRedirectToBudgets()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);
        var budget = new Budget
        {
            CategoryId = 1,
            Sum = 1000,
            Guid = Guid.NewGuid()
        };
        context.Budgets.Add(budget);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.DeleteBudget(budget.Guid.ToString());

        // Assert
        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.Equal("Budgets", redirectResult.Url);
    }

    [Fact]
    public async Task DetailsBudget_ShouldReturnView()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);
        var budget = new Budget
        {
            CategoryId = 1,
            Sum = 1000,
            Guid = Guid.NewGuid()
        };
        context.Budgets.Add(budget);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.DetailsBudget(budget.Guid.ToString());

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        BudgetVM? vm = viewResult.Model as BudgetVM;
        if (vm == null)
            Assert.Fail("Wrong model");

        Assert.Equal(budget.Guid.ToString(), vm.Guid);
    }

    [Fact]
    public async Task DeleteBudget_ShouldThrowNotFound()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var service = new BudgetService(context);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            // Act
            await service.Delete(Guid.NewGuid());
        });
    }

    [Fact]
    public async Task EditBudget_ShouldThrowNotFound()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var service = new BudgetService(context);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            // Act
            await service.Edit(Guid.NewGuid(), new Budget
            {
                CategoryId = 1,
                Sum = 1000
            });
        });
    }

    [Fact]
    public async Task GetBudget_ShouldThrowNotFound()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var service = new BudgetService(context);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            // Act
            await service.Get(Guid.NewGuid());
        });
    }

    [Fact]
    public async Task Budgets_ShouldReturnBadRequest_WhenUserNotAuthenticated()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext() 
        };

        // Act
        var result = await controller.Budgets();

        // Assert
        Assert.IsType<BadRequestObjectResult>(result); 
    }



    [Fact]
    public async Task DetailsBudget_ShouldThrowNotFound()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            // Act
            await controller.DetailsBudget(Guid.NewGuid().ToString());
        });
    }

    [Fact]
    public async Task DetailsBudget_ShouldReturnBadRequest()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act
        var result = await controller.DetailsBudget(string.Empty);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Budgets_ShouldReturnEmptyListWhenNoBudgetsExist()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act
        var result = await controller.Budgets();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<BudgetVM>>(viewResult.ViewData.Model);
        Assert.Empty(model);
    }

    [Fact]
    public async Task Budgets_ShouldOnlyReturnBudgetsForLoggedInUser()
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

        var budget1 = new Budget
        {
            Guid = Guid.NewGuid(),
            CategoryId = 1,
            UserId = user1.Iduser,
            Sum = 1000
        };
        var budget2 = new Budget
        {
            Guid = Guid.NewGuid(),
            CategoryId = 1,
            UserId = user2.Iduser,
            Sum = 2000
        };

        await context.Budgets.AddRangeAsync(budget1, budget2);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.Budgets();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<BudgetVM>>(viewResult.ViewData.Model);
        Assert.Single(model);
        Assert.Equal(budget1.Guid.ToString(), model.First().Guid);
    }

    [Fact]
    public async Task DeleteBudget_ShouldRemoveBudgetFromDatabase()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        var budget = new Budget
        {
            Guid = Guid.NewGuid(),
            Idbudget = 1,
            Sum = 1000
        };

        await context.Budgets.AddAsync(budget);
        await context.SaveChangesAsync();

        // Act
        await controller.DeleteBudget(budget.Guid.ToString());

        var result = await context.Budgets.FirstOrDefaultAsync(b => b.Guid == budget.Guid);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Budgets_ShouldReturnBadRequestWhenGuidIsInvalid()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        // Act
        var result = await controller.Budgets();

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task EditBudget_ShouldThrowNotFoundWhenGuidIsInvalid()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await controller.EditBudget(Guid.Empty.ToString()); 
        });
    }



    [Fact]
    public async Task DetailsBudget_ShouldHandleNullGuidGracefully()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act
        var result = await controller.DetailsBudget(null);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task DeleteBudget_ShouldThrowNotFoundWhenGuidIsInvalid()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await controller.DeleteBudget(Guid.Empty.ToString()); 
        });
    }

    [Fact]
    public async Task Budgets_ShouldThrowExceptionWhenDatabaseFails()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        await context.DisposeAsync();

        // Act & Assert
        await Assert.ThrowsAsync<ObjectDisposedException>(async () =>
        {
            await controller.Budgets();
        });
    }


    [Fact]
    public async Task DeleteBudget_ShouldHandleNotFoundException()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await controller.DeleteBudget(Guid.NewGuid().ToString()); 
        });
    }

    [Fact]
    public async Task DetailsBudget_ShouldHandleNotFoundException()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await controller.DetailsBudget(Guid.NewGuid().ToString()); 
        });
    }





}
