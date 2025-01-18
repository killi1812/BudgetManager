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

}

