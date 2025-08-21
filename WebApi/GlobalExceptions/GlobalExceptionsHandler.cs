using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.GlobalExceptions;

public class GlobalExceptionsHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionsHandler> _logger;

    public GlobalExceptionsHandler(ILogger<GlobalExceptionsHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception ocurred: {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server error"
        };

        context.Response.StatusCode = problemDetails.Status.Value;

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}