using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        int statusCode;

        if (exception is KeyNotFoundException)
        {
            statusCode = StatusCodes.Status404NotFound;
        }
        else if (exception is ArgumentException)
        {
            statusCode = StatusCodes.Status400BadRequest;
        }
        else
        {
            statusCode = StatusCodes.Status500InternalServerError;
        }

        httpContext.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = "An error occurred",
            Detail = exception.Message
        };

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}