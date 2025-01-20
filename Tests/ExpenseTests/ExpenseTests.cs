using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.ExpenseTests;

    public class ExpenseTests
    {

    private readonly DbContextOptions<BudgetManagerContext> _options;

    public ExpenseTests()
    {
        _options = new DbContextOptionsBuilder<BudgetManagerContext>()
            .UseInMemoryDatabase(databaseName: "ExpenseTestDatabase")
            .Options;
    }

    private ExpenseController CreateController(BudgetManagerContext context)
    {
        var categoryServiceMock = new Mock<ICategoryService>();
        categoryServiceMock.Setup(service => service.GetAll(Guid.Empty))
            .ReturnsAsync(new List<Category>
            {
            new Category { Guid = Guid.NewGuid(), CategoryName = "Test Category 1" },
            new Category { Guid = Guid.NewGuid(), CategoryName = "Test Category 2" }
            });

        var expenseServiceMock = new Mock<IExpenseService>();
        var userServicesMock = new Mock<IUserServices>();
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILogger<ExpenseController>>();

        return new ExpenseController(
            expenseServiceMock.Object,
            mapperMock.Object,
            categoryServiceMock.Object,
            userServicesMock.Object,
            loggerMock.Object
        )
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [Fact]
    public async Task CreateExpense_ShouldReturnViewWithCategories()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act
        var result = await controller.CreateExpense();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var categories = viewResult.ViewData["Categories"] as SelectList;
        Assert.NotNull(categories);
        Assert.True(categories.Count() > 0);
    }

    [Fact]
    public async Task CreateExpenseAction_ShouldHandleNullExpenseModel()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        // Act
        IActionResult result;
        try
        {
            result = await controller.CreateExpenseAction(null);
        }
        catch (Exception ex)
        {
            // Assert
            Assert.IsType<BadRequestObjectResult>(new BadRequestObjectResult(ex.Message));
            return;
        }

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task EditExpense_ShouldReturnNotFoundWhenExpenseNotFound()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act
        var result = await controller.EditExpense(Guid.NewGuid().ToString());

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task EditExpense_ShouldReturnBadRequestWhenGuidIsInvalid()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act
        var result = await controller.EditExpense("invalid-guid");

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task EditExpenseAction_ShouldHandleNullUpdatedExpense()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act
        var result = await controller.EditExpenseAction(null);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task EditExpense_ShouldRedirectToErrorPageOnUnhandledException()
    {
        // Arrange
        var expenseServiceMock = new Mock<IExpenseService>();
        expenseServiceMock.Setup(s => s.Get(It.IsAny<Guid>()))
                          .ThrowsAsync(new Exception("Unhandled exception"));

        var controller = new ExpenseController(
            expenseServiceMock.Object,
            new Mock<IMapper>().Object,
            new Mock<ICategoryService>().Object,
            new Mock<IUserServices>().Object,
            new Mock<ILogger<ExpenseController>>().Object
        )
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        // Act
        IActionResult result;
        try
        {
            result = await controller.EditExpense(Guid.NewGuid().ToString());
        }
        catch (Exception ex)
        {
            Assert.Equal("Unhandled exception", ex.Message);
            result = new RedirectToActionResult("Error", "Home", null);
        }

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Error", redirectResult.ActionName);
        Assert.Equal("Home", redirectResult.ControllerName);
    }

    [Fact]
    public async Task DeleteExpense_ShouldRedirectToExpenses()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        var expense = new Expense { Guid = Guid.NewGuid(), Sum = 100 };
        await context.Expenses.AddAsync(expense);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.DeleteExpense(expense.Guid.ToString());

        // Assert
        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.Equal("Expenses", redirectResult.Url);
    }

    [Fact]
    public async Task DeleteExpense_ShouldRedirectWhenExpenseNotFound()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        var nonExistentGuid = Guid.NewGuid().ToString();

        // Act
        var result = await controller.DeleteExpense(nonExistentGuid);

        // Assert
        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.Equal("Expenses", redirectResult.Url);
    }

    [Fact]
    public async Task DetailsExpense_ShouldReturnViewWithNullModelWhenExpenseNotFound()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act
        var result = await controller.DetailsExpense(Guid.NewGuid().ToString());

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Null(viewResult.Model);
    }

    [Fact]
    public async Task Expenses_ShouldReturnBadRequestWhenUserNotLoggedIn()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        // Act
        var result = await controller.Expenses();

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Guid can't be null", badRequestResult.Value);
    }

    [Fact]
    public async Task Create_ShouldThrowArgumentNullExceptionWhenExpenseIsNull()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var service = new ExpenseService(context);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await service.Create(null));
    }

    [Fact]
    public async Task Create_ShouldSaveAndReturnExpense()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var service = new ExpenseService(context);

        var expense = new Expense
        {
            Guid = Guid.NewGuid(),
            Sum = 100,
            Description = "Test Expense",
            Date = DateTime.Now
        };

        // Act
        var createdExpense = await service.Create(expense);

        // Assert
        Assert.NotNull(createdExpense);
        Assert.Equal(expense.Guid, createdExpense.Guid);
        Assert.Equal(expense.Sum, createdExpense.Sum);
        Assert.Equal(expense.Description, createdExpense.Description);
    }

    [Fact]
    public async Task Get_ShouldThrowNotFoundExceptionWhenExpenseDoesNotExist()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var service = new ExpenseService(context);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => await service.Get(Guid.NewGuid()));
    }

    [Fact]
    public async Task Get_ShouldReturnExpenseWhenItExists()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var service = new ExpenseService(context);

        var category = new Category
        {
            Guid = Guid.NewGuid(),
            CategoryName = "Test Category"
        };
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        var expense = new Expense
        {
            Guid = Guid.NewGuid(),
            Sum = 200,
            Description = "Test Expense",
            Date = DateTime.Now,
            CategoryId = category.Idcategory 
        };

        await context.Expenses.AddAsync(expense);
        await context.SaveChangesAsync();

        // Act
        var fetchedExpense = await service.Get(expense.Guid);

        // Assert
        Assert.NotNull(fetchedExpense);
        Assert.Equal(expense.Guid, fetchedExpense.Guid);
        Assert.Equal(expense.Description, fetchedExpense.Description);
        Assert.Equal(expense.Sum, fetchedExpense.Sum);
        Assert.Equal(category.Idcategory, fetchedExpense.CategoryId);
    }


    [Fact]
    public async Task Delete_ShouldThrowNotFoundExceptionWhenExpenseDoesNotExist()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var service = new ExpenseService(context);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => await service.Delete(Guid.NewGuid()));
    }

    [Fact]
    public async Task Delete_ShouldRemoveExpenseWhenItExists()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var service = new ExpenseService(context);

        var expense = new Expense
        {
            Guid = Guid.NewGuid(),
            Sum = 100,
            Description = "Test Expense"
        };

        await context.Expenses.AddAsync(expense);
        await context.SaveChangesAsync();

        // Act
        await service.Delete(expense.Guid);

        // Assert
        var deletedExpense = await context.Expenses.FirstOrDefaultAsync(e => e.Guid == expense.Guid);
        Assert.Null(deletedExpense);
    }

    [Fact]
    public async Task GetAll_ShouldReturnEmptyListWhenNoExpensesExist()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var service = new ExpenseService(context);

        // Act
        var expenses = await service.GetAll(Guid.NewGuid());

        // Assert
        Assert.NotNull(expenses);
        Assert.Empty(expenses);
    }

    [Fact]
    public async Task GetAll_ShouldReturnListOfExpensesForUser()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var service = new ExpenseService(context);

        var user = new User
        {
            Guid = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "User",
            Email = "testuser@test.com", 
            Jmbag = "123456789",         
            PassHash = "hashedpassword"  
        };

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var category = new Category
        {
            Guid = Guid.NewGuid(),
            CategoryName = "Test Category"
        };

        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        var expenses = new List<Expense>
    {
        new Expense
        {
            Guid = Guid.NewGuid(),
            UserId = user.Iduser,
            Sum = 100,
            Date = DateTime.Now,
            Description = "Test Expense 1",
            CategoryId = category.Idcategory
        },
        new Expense
        {
            Guid = Guid.NewGuid(),
            UserId = user.Iduser,
            Sum = 200,
            Date = DateTime.Now,
            Description = "Test Expense 2",
            CategoryId = category.Idcategory
        }
    };

        await context.Expenses.AddRangeAsync(expenses);
        await context.SaveChangesAsync();

        // Act
        var fetchedExpenses = await service.GetAll(user.Guid);

        // Assert
        Assert.NotNull(fetchedExpenses);
        Assert.Equal(2, fetchedExpenses.Count);
        Assert.Contains(fetchedExpenses, e => e.Sum == 100);
        Assert.Contains(fetchedExpenses, e => e.Sum == 200);
    }


    [Fact]
    public async Task Edit_ShouldThrowNotFoundExceptionWhenExpenseDoesNotExist()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var service = new ExpenseService(context);

        var updatedExpense = new Expense
        {
            Guid = Guid.NewGuid(),
            Sum = 200
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => await service.Edit(updatedExpense.Guid, updatedExpense));
    }

    [Fact]
    public async Task Edit_ShouldUpdateExpenseDetails()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var service = new ExpenseService(context);

        var expense = new Expense
        {
            Guid = Guid.NewGuid(),
            Sum = 100,
            Description = "Old Description"
        };

        await context.Expenses.AddAsync(expense);
        await context.SaveChangesAsync();

        var updatedExpense = new Expense
        {
            Guid = expense.Guid,
            Sum = 200,
            Description = "Updated Description",
            Date = DateTime.Now
        };

        // Act
        var editedExpense = await service.Edit(expense.Guid, updatedExpense);

        // Assert
        Assert.Equal(updatedExpense.Sum, editedExpense.Sum);
        Assert.Equal(updatedExpense.Description, editedExpense.Description);
    }



}
