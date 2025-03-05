using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TodoList.Application.Common.Exceptions;

namespace TodoList;

public class CustomExceptionsHandler : IExceptionHandler
{
    private readonly IDictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;

    public CustomExceptionsHandler()
    {
        _exceptionHandlers = new Dictionary<Type, Func<HttpContext, Exception, Task>>()
        {
            {typeof(NotFoundException), HandleNotFoundException},
            {typeof(ValidationException), HandleValidationException}
        };
    }
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var exceptionType = exception.GetType();
        if (!_exceptionHandlers.ContainsKey(exceptionType)) return false;
        
        var handler = _exceptionHandlers[exceptionType];
        await handler.Invoke(httpContext, exception);
        
        return true;
    }

    private async Task HandleNotFoundException(HttpContext httpContext, Exception exception)
    {
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Title = "Not found",
            Status = StatusCodes.Status404NotFound,
            Detail = "Requested resource does not exist"
        });
    }

    private async Task HandleValidationException(HttpContext httpContext, Exception exception)
    {
        var validationException = (ValidationException)exception;
        
        await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails
        {
            Title = validationException.Message,
            Errors = validationException.Errors,
            Status = StatusCodes.Status400BadRequest
        });
    }
}