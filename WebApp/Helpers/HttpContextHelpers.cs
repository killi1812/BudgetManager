using Data.Helpers;

namespace WebApp.Helpers;

public static class HttpContextHelpers
{
   public static Guid GetUserGuid(this HttpContext context)
   {
        var guid = context.User.Claims.FirstOrDefault(c => c.Type == "UserGuid")?.Value;
        if (guid == null)
            throw new UnauthorizedException("Guid can't be found");
        return Guid.Parse(guid);
   }
}