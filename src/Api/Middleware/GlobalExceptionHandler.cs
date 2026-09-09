using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TeamworkApp.Application.Auth;
using TeamworkApp.Application.Posts;

namespace TeamworkApp.Api.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, title, detail) = exception switch
        {
            EmailAlreadyInUseException => (StatusCodes.Status409Conflict, "Email Already In Use", exception.Message),
            InvalidCredentialsException => (StatusCodes.Status401Unauthorized, "Invalid Credentials", exception.Message),
            ArticleNotFoundException => (StatusCodes.Status404NotFound, "Article Not Found", exception.Message),
            GifNotFoundException => (StatusCodes.Status404NotFound, "Gif Not Found", exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred", "An unexpected error occurred. Please try again later.")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception.");
        }

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail
        }, cancellationToken);

        return true;
    }
}
