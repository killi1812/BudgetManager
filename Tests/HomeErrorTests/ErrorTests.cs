namespace Tests.HomeErrorTests;

public class ErrorControllerTests
{
    private ErrorController CreateController()
    {
        return new ErrorController();
    }

    [Fact]
    public void Error404_ShouldReturnView()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.Error404("Page not found");

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Error404_ShouldSetMessageInViewBag()
    {
        // Arrange
        var controller = CreateController();
        var message = "Page not found";

        // Act
        var result = controller.Error404(message);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(message, controller.ViewBag.Message);
    }

    [Fact]
    public void Error401_ShouldReturnView()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.Error401("Unauthorized access");

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Error401_ShouldSetMessageInViewBag()
    {
        // Arrange
        var controller = CreateController();
        var message = "Unauthorized access";

        // Act
        var result = controller.Error401(message);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(message, controller.ViewBag.Message);
    }

    [Fact]
    public void Error500_ShouldReturnView()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = controller.Error500("Internal server error");

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Error500_ShouldSetMessageInViewBag()
    {
        // Arrange
        var controller = CreateController();
        var message = "Internal server error";

        // Act
        var result = controller.Error500(message);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(message, controller.ViewBag.Message);
    }
}

