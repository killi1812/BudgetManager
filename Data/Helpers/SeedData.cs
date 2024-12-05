using Data.Dto;
using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Helpers;

public static class SeedDataClass
{
    public static async Task<IApplicationBuilder> SeedData(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app, nameof(app));
        using var scope = app.ApplicationServices.CreateScope();
        IServiceProvider services = scope.ServiceProvider;
        var context = services.GetRequiredService<RwaContext>();
        if (context.Users.Any())
            return app;
        await SeedUsers(services);
        return app;
    }

    private static async Task SeedUsers(IServiceProvider services)
    {
        var UserServices = services.GetRequiredService<IUserServices>();

        await UserServices.CreateUser(new NewUserDto
        {
            Username = "admin",
            Password = "admin",
            Admin = true
        });
        await UserServices.CreateUser(new NewUserDto
        {
            Username = "admin2",
            Password = "admin",
            Admin = true
        });
        await UserServices.CreateUser(new NewUserDto
        {
            Username = "pero",
            Password = "123",
            Admin = false
        });
        await UserServices.CreateUser(new NewUserDto
        {
            Username = "user",
            Password = "user",
            Admin = false
        });
    }

    private static void SeedPictures()
    {
    }
}