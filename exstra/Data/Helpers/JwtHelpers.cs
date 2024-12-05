using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace Data.Helpers;

public static class JwtHelpers
{
    public static Guid? GetGuid(this HttpRequest request)
    {
        {
            if (!request.Headers.ContainsKey("Authorization"))
                return null;
            string authorizationHeader = request.Headers["Authorization"];
            if (!authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return null;
            string token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "guid");
            return Guid.Parse(userIdClaim?.Value!);
        }
    }
}