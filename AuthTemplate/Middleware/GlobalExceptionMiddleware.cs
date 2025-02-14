namespace Auth.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
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
            Console.WriteLine($"ERROR CRÍTICO: {ex.Message}");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Error interno del servidor.");
        }
    }
}
