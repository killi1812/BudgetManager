using System.Security.Claims;
using AutoMapper;
using Data.Helpers;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Controllers;
using WebApp.Helpers;
using WebApp.ViewModels;

namespace Tests.CategoryTests;

public class CategoryControllerTests
{
    private readonly DbContextOptions<BudgetManagerContext> _options;
    private readonly IMapper _mapper;

    public CategoryControllerTests()
    {
        _options = new DbContextOptionsBuilder<BudgetManagerContext>()
            .UseInMemoryDatabase(databaseName: "CategoryTestDatabse")
            .Options;

        var config = new MapperConfiguration(cfg => { cfg.AddProfile<MapperProfile>(); });

        _mapper = config.CreateMapper();
    }

    private CategoryController CreateController(BudgetManagerContext context)
    {
        var categoryService = new CategoryService(context);
        var controller = new CategoryController(categoryService, _mapper);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim("UserGuid", Guid.NewGuid().ToString())
        }, "mock"));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        return controller;
    }

    [Fact]
    public async Task Categories_ShouldReturnViewWithCategories()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act
        var result = await controller.Categories();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.IsAssignableFrom<List<CategoryVM>>(viewResult.ViewData.Model);
    }

    [Fact]
    public async Task CreateCategory_ShouldReturnView()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);

        // Act
        var result = controller.CreateCategory();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task CreateCategoryAction_ShouldRedirectToCategories()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);
        var newCategory = new CategoryVM
        {
            Name = "Test Category",
            Color = "Red"
        };

        // Act
        var result = await controller.CreateCategoryAction(newCategory);

        // Assert
        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.Equal("Categories", redirectResult.Url);

        // Clean
        context.Remove(context.Categories.First());
        await context.SaveChangesAsync();
    }


    [Fact]
    public async Task EditCategory_ShouldReturnView()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);
        var newCategory = new Category
        {
            CategoryName = "Test Category",
            Color = "Red"
        };
        await context.AddAsync(newCategory);
        await context.SaveChangesAsync();
        // Act
        var result = await controller.EditCategory(newCategory.Guid.ToString());

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        CategoryVM? vm = viewResult.Model as CategoryVM;
        if (vm == null)
            Assert.Fail("Wrong model");

        Assert.Equal(newCategory.Guid.ToString(), vm.Guid);

        // Clean
        context.Remove(context.Categories.First());
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task EditCategoryAction_ShouldRedirectToCategories()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);
        var category = new Category
        {
            CategoryName = "Test Category",
            Color = "Red"
        };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var updatedCategory = new CategoryVM
        {
            Guid = category.Guid.ToString(),
            Name = "Updated Category",
            Color = "Blue"
        };

        // Act
        var result = await controller.EditCategoryAction(updatedCategory);

        // Assert
        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.Equal("Categories", redirectResult.Url);

        // Clean
        context.Remove(context.Categories.First());
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteCategory_ShouldRedirectToCategories()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);
        var category = new Category
        {
            CategoryName = "Test Category",
            Color = "Red"
        };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.DeleteCategory(category.Guid.ToString());

        // Assert
        var redirectResult = Assert.IsType<RedirectResult>(result);
        Assert.Equal("Categories", redirectResult.Url);
    }

    [Fact]
    public async Task GetPropsCategory_ShouldReturnProps()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var controller = CreateController(context);
        var category = new Category
        {
            CategoryName = "Test Category",
            Color = "Red"
        };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var props = new List<PropDto>
        {
            new()
            {
                Text = category.CategoryName,
                Value = category.Guid.ToString()
            }
        };

        // Act 
        var result = await controller.CategoryProps();

        // Assert
        var propResult = Assert.IsType<JsonResult>(result);
        Assert.Equal(props, propResult.Value);

        // Clean 
        context.Remove(context.Categories.First());
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetCategory_ShouldThrowNotFound()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var service = new CategoryService(context);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            // Act
            await service.Get(Guid.NewGuid());
        });
    }
    
    [Fact]
    public async Task DeleteCategory_ShouldThrowNotFound()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var service = new CategoryService(context);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            // Act
            await service.Delete(Guid.NewGuid());
        });
    }
    [Fact]
    public async Task EditCategory_ShouldThrowNotFound()
    {
        // Arrange
        await using var context = new BudgetManagerContext(_options);
        var service = new CategoryService(context);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            // Act
            await service.Edit(new Category
            {
            CategoryName = "Test Category",
            Color = "Red"
            });
        });
    }
}