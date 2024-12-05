using System.Net;
using Data.Dto;
using Data.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Helpers;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
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

        //More log stuff        
        ExceptionResponse response = null;
        switch (exception)
        {
            case NotFoundException:
                response = new ExceptionResponse(HttpStatusCode.NotFound, exception.Message);
                _loggerService.Log(exception.Message, ThreatLvl.Medium);
                break;
            case UnauthorizedException:
                response = new ExceptionResponse(HttpStatusCode.Unauthorized, exception.Message);
                _loggerService.Log(exception.Message, ThreatLvl.High);
                break;
            default:
                response = new ExceptionResponse(HttpStatusCode.InternalServerError, exception.Message);
                _loggerService.Log(exception.Message, ThreatLvl.High);
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.Status;
        await context.Response.WriteAsync(response.ToJson());
    }
}