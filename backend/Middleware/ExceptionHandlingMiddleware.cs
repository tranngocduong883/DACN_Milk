using DACN_H_P.Utils;

namespace DACN_H_P.Middleware
{
    // Bắt mọi exception chưa được xử lý, ghi log và trả về APIResponse thống nhất thay vì stack trace
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xử lý được tại {Method} {Path}", context.Request.Method, context.Request.Path);

                if (context.Response.HasStarted)
                {
                    throw;
                }

                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(APIResponse<object>.Fail("Đã xảy ra lỗi hệ thống, vui lòng thử lại sau"));
            }
        }
    }
}
