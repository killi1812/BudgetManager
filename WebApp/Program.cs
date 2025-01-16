using Data.Helpers;
using Data.Models;
using Data.Services;
using Microsoft.EntityFrameworkCore;
using PaymentService.Builder;
using WebApp.Helpers;

var paymentService = new PaymentServiceBuilder()
    .WithCheckInterval(TimeSpan.FromSeconds(3))
    .WithStartTime(new TimeOnly(8, 0))
    .WithStopTime(new TimeOnly(16, 0))
    .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BudgetManagerContext>(o =>
{
    o.UseSqlServer(builder.Configuration["ConnectionStrings:db"]);
});
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication()
    .AddCookie(o =>
    {
        o.LoginPath = "/Auth/Login";
        o.LogoutPath = "/Auth/Logout";
        o.AccessDeniedPath = "/Auth/AccessDenied";
        o.SlidingExpiration = true;
        o.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<ILoggerService, LoggerService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IIncomeService, IncomeService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<ISavingService, SavingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    await app.SeedData();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseMiddleware<ExceptionRedirectMiddleware>();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

paymentService.Run();
app.Run();