using System.Net;
using Data.Helpers;
using Data.Services;

namespace WebApp.Helpers;

public class ExceptionRedirectMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionRedirectMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var _loggerService = context.RequestServices.GetService<ILoggerService>();

        string redirectUrl = "/Error";
        var message = Uri.EscapeDataString(exception.Message);
        switch (exception)
        {
            case NotFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                _loggerService.Log(exception.Message, ThreatLvl.Medium);
                redirectUrl = $"/Error/Error404?message={message}";
                break;
            case UnauthorizedException:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                _loggerService.Log(exception.Message, ThreatLvl.High);
                redirectUrl = $"/Error/Error401?message={Uri.EscapeDataString(exception.Message)}";
                break;
            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                _loggerService.Log(exception.Message, ThreatLvl.High);
                redirectUrl = $"/Error/Error500?message={Uri.EscapeDataString(exception.Message)}";
                break;
        }

        context.Response.Redirect(redirectUrl);
    }
}