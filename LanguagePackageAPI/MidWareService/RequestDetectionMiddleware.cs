using Helpers;

namespace MidWareService
{
    public class RequestDetectionMiddleware
    {
        private readonly RequestDelegate _next;

        private static IConfiguration configuration;


        public RequestDetectionMiddleware(RequestDelegate next, IConfiguration _configuration)
        {
            _next = next;
            configuration = _configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Request bilgilerini işleme alabilirsiniz
            var requestPath = context.Request.Path;
            var requestMethod = context.Request.Method;
            //var connLocal = configuration.GetConnectionString("ConnLocalDb");
            var connReal = configuration.GetConnectionString("ConnRealDb");
            SqlConnHelper.SetDbConn(connReal);
            // Özel işlemlerinizi burada gerçekleştirin

            // Sonraki middleware'e geç
            await _next(context);
        }
    }
}
