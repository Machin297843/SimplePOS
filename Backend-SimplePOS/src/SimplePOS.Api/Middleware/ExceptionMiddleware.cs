using System.Net;
using SimplePOS.Application.Common.Errors;

namespace SimplePOS.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (ArgumentException ex)
        {
            await Write(ctx, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (NotFoundException ex)
        {
            await Write(ctx, HttpStatusCode.NotFound, ex.Message);
        }
        catch (ConflictException ex)
        {
            await Write(ctx, HttpStatusCode.Conflict, ex.Message);
        }
    }

    private static Task Write(HttpContext ctx, HttpStatusCode code, string message)
    {
        ctx.Response.StatusCode = (int)code;
        ctx.Response.ContentType = "application/json";
        return ctx.Response.WriteAsJsonAsync(new { message });
    }
}