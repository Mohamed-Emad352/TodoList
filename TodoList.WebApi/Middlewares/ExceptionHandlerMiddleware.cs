using Microsoft.AspNetCore.Mvc;
using TodoList.Application.Common.Exceptions;

namespace TodoList.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
        _exceptionHandlers = new Dictionary<Type, Func<HttpContext, Exception, Task>>()
        {
            {typeof(NotFoundException), HandleNotFoundException},
            {typeof(ValidationException), HandleValidationException}
        };
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            var exceptionType = exception.GetType();
            if (!_exceptionHandlers.ContainsKey(exceptionType)) return;
        
            var handler = _exceptionHandlers[exceptionType];
            await handler.Invoke(context, exception);
        }
    }

    private async Task HandleNotFoundException(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Title = "Not found",
            Detail = "Requested resource does not exist"
        });
    }

    private async Task HandleValidationException(HttpContext httpContext, Exception exception)
    {
        var validationException = (ValidationException)exception;
        
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        
        await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails
        {
            Title = validationException.Message,
            Errors = validationException.Errors,
        });
    }
}

public static class ExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlerMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}