using System.Runtime.InteropServices.JavaScript;
using Data.Enums;

namespace Tests.CategoryTests;

public class IncomeControllerTests
{
    private readonly DbContextOptions<BudgetManagerContext> _options;
    private readonly IMapper _mapper;

    public IncomeControllerTests()
    {
        _options = new DbContextOptionsBuilder<BudgetManagerContext>()
            .UseInMemoryDatabase(databaseName: "IncomeTestDatabase")
            .Options;
        _mapper = TestSetupHelpers.SetupAutomapper();
    }

    private IncomeController CreateController(BudgetManagerContext context)
    {
        var incomeService = new IncomeService(context, _mapper);
        var userService = new UserServices(_mapper, context);
        var controller = new IncomeController(incomeService, _mapper, userService);
        var user = new User
        {
            FirstName = "Test",
            LastName = "Test",
            Jmbag = "1231123412",
            Email = "test@test.com",
            PassHash = "123"
        };
        context.Users.Add(user);
        context.SaveChanges();
        controller.MockLoginUser(user.Guid);
        return controller;
    }

    [Fact]
    public async Task Incomes_ShouldReturnViewWithIncomes()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act
        var result = await controller.Incomes();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.IsAssignableFrom<List<IncomeVM>>(viewResult.ViewData.Model);
    }

    [Fact]
    public async Task Incomes_ShouldThrowUnauthorized()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller =
            new IncomeController(new IncomeService(context, _mapper), _mapper, new UserServices(_mapper, context));
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        // Assert
        await Assert.ThrowsAsync<UnauthorizedException>(async () =>
        {
            // Act
            await controller.Incomes();
        });
    }

    [Fact]
    public async Task CreateIncome_ShouldReturnView()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act
        var result = controller.CreateIncome();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task CreateIncomeAction_ShouldCreateIncome()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);
        var newIncome = new IncomeVM
        {
            Sum = 1000,
            Source = "Test Income",
            Date = DateTime.Today,
            Frequency = Frequency.Once
        };


        // Act
        var result = await controller.CreateIncomeAction(newIncome);

        // Assert
        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.Equal("Incomes", redirectResult.Url);

        // Clean
        context.Remove(context.Incomes.First());
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task CreateIncomeAction_ShouldThrowUnauthorized()
    {
             // Arrange
             await using var context = new BudgetManagerContext(_options);
             var controller = new IncomeController(new IncomeService(context, _mapper), _mapper, new UserServices(_mapper, context));
             controller.ControllerContext = new ControllerContext
             {
                 HttpContext = new DefaultHttpContext()
             };
                     var newIncome = new IncomeVM
                     {
                         Sum = 1000,
                         Source = "Test Income",
                         Date = DateTime.Today,
                         Frequency = Frequency.Once
                     };

             // Assert
             await Assert.ThrowsAsync<UnauthorizedException>(async () =>
             {
                 // Act
                 await controller.CreateIncomeAction(newIncome);
             });   
    }

    [Fact]
    public async Task EditIncome_ShouldReturnView()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);
        var newIncome = new Income
        {
            Sum = 1000,
            Source = "Test Income",
            Date = DateTime.Today,
            Frequency = Frequency.Once.ToString(),
        };
        await context.AddAsync(newIncome);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.EditIncome(newIncome.Guid.ToString());

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        IncomeVM? vm = viewResult.Model as IncomeVM;
        if (vm == null)
            Assert.Fail("Wrong model");

        Assert.Equal(newIncome.Guid.ToString(), vm.Guid);

        // Clean
        context.Remove(context.Incomes.First());
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task EditIncome_ShouldThrowNotFound()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            // Act
            await controller.EditIncome(Guid.NewGuid().ToString());
        });
    }

    [Fact]
    public async Task EditIncomeAction_ShouldRedirectToIncomes()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);
        var income = new Income
        {
            Sum = 1000,
            Source = "Test Income",
            Date = DateTime.Today,
            Frequency = Frequency.Once.ToString(),
        };
        context.Incomes.Add(income);
        await context.SaveChangesAsync();

        var updatedIncome = new IncomeVM
        {
            Guid = income.Guid.ToString(),
            Sum = 1000,
            Source = "Test Income",
            Date = DateTime.Today,
            Frequency = Frequency.Once
        };

        // Act
        var result = await controller.EditIncomeAction(updatedIncome);

        // Assert
        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.Equal("Incomes", redirectResult.Url);

        // Clean
        context.Remove(context.Incomes.First());
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task EditIncomeAction_ShouldThrowNotFound()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);
        var updatedIncome = new IncomeVM
        {
            Guid = Guid.NewGuid().ToString(),
            Sum = 1000,
            Source = "Test Income",
            Date = DateTime.Today,
            Frequency = Frequency.Once
        };

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            // Act
            await controller.EditIncomeAction(updatedIncome);
        });
    }

    [Fact]
    public async Task DeleteIncome_ShouldRedirectToIncomes()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);
        var income = new Income
        {
            Sum = 1000,
            Source = "Test Income",
            Date = DateTime.Today,
            Frequency = Frequency.Once.ToString(),
        };
        context.Incomes.Add(income);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.DeleteIncome(income.Guid.ToString());

        // Assert
        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.Equal("Incomes", redirectResult.Url);
    }

    [Fact]
    public async Task DeleteIncomeAction_ShouldThrowNotFound()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            // Act
            await controller.DeleteIncome(Guid.NewGuid().ToString());
        });
    }
}