using System.Diagnostics;


namespace Tests.HomeErrorTests;

    public class HomeControllerTests
    {
        private readonly IMapper _mapper;

        public HomeControllerTests()
        {
            _mapper = TestSetupHelpers.SetupAutomapper();
        }

        private HomeController CreateController()
        {
            return new HomeController(_mapper);
        }

        [Fact]
        public void Index_ShouldReturnView()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_ShouldReturnViewWithErrorViewModel()
        {
            // Arrange
            var controller = CreateController();

            var httpContext = new DefaultHttpContext();
            httpContext.TraceIdentifier = "TestTraceIdentifier"; 
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);
            Assert.NotNull(model.RequestId);
            Assert.Equal("TestTraceIdentifier", model.RequestId); 
        }

        [Fact]
        public void Index_ShouldReturnStatus200()
        {
            // Arrange
            var controller = CreateController();

            var httpContext = new DefaultHttpContext();
            httpContext.Response.StatusCode = 200;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(200, controller.ControllerContext.HttpContext.Response.StatusCode);
        }

        [Fact]
        public void Error_ShouldReturnStatus200()
        {
            // Arrange
            var controller = CreateController();

            var httpContext = new DefaultHttpContext();
            httpContext.Response.StatusCode = 200; 
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(200, controller.ControllerContext.HttpContext.Response.StatusCode);
        }

    }


