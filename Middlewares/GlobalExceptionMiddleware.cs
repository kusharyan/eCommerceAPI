using eCommerceApi.Exceptions;

namespace eCommerceApi.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger, RequestDelegate next)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unhandled exception | Path: {Path} | Method: {Method} | TraceId: {TraceId}", context.Request.Path, context.Request.Method, context.TraceIdentifier);
                await HandleExceptionsAsync(context, exception);
            }
        }

        private static Task HandleExceptionsAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            if(exception is AppExcepton appExcepton)
            {
                context.Response.StatusCode = appExcepton.StatusCode;

                return context.Response.WriteAsJsonAsync(new
                {
                    message = appExcepton.Message,
                    traceId = context.TraceIdentifier
                });
            }

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            return context.Response.WriteAsJsonAsync(new
            {
                message = "Internal Server Error",
                traceId = context.TraceIdentifier
            });
        }
    }
}