namespace LR5.Models
{
    public class MiddlewareErrorLogger
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MiddlewareErrorLogger> _logger;

        public MiddlewareErrorLogger(RequestDelegate next, ILogger<MiddlewareErrorLogger> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Виклик наступного delegate/middleware у пайплайні
                await _next(context);
            }
            catch (Exception e)
            {
                // Логування помилки
                _logger.LogError(e, $"{e.Message}");

                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync($"An unexpected error occurred! Exception: {e.Message}");
            }
        }
    }
}
