namespace Tests.TestHelpers;

public static class TestSetupHelpers
{
   public static IMapper SetupAutomapper()
   {
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<MapperProfile>(); });
        return config.CreateMapper();
   }

   public static Controller MockLoginUser(this Controller controller, Guid? userGuid = null)
   {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new("UserGuid", userGuid?.ToString() ?? Guid.NewGuid().ToString())
        }, "mock"));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
        return controller;
   }
}