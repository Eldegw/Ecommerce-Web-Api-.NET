using Ecom.Api.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace Ecom.Api.MiddelWare
{
    public class ExceptionMiddelware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30);
         
        public ExceptionMiddelware(RequestDelegate next, IHostEnvironment environment, IMemoryCache memoryCache)
        {
            _next = next;
            _environment = environment;
            _memoryCache = memoryCache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (!IsAllowedRwquest(context))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";

                    var respone = new ResponseApi((int)HttpStatusCode.TooManyRequests, "Too Many Requst,Please Try Again Later");
                    await context.Response.WriteAsJsonAsync(respone);

                }

                await _next(context);

            }
            catch( Exception ex)  
            {
              context.Response.StatusCode= (int)HttpStatusCode.InternalServerError;
              context.Response.ContentType = "application/json";
              
                var respone = _environment.IsDevelopment()?
                    new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                    : new ApiException((int)HttpStatusCode.InternalServerError, ex.Message);

                var json = JsonSerializer.Serialize(respone);
                await context.Response.WriteAsync(json);
             
            }

        }

        private bool IsAllowedRwquest(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress;
            var cachKey = $"Rate:{ip}";
            var dateNow = DateTime.Now;

            var (timesTamp, count) = _memoryCache.GetOrCreate(cachKey, entry => 
            {
                entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
                return (timesTamp: dateNow, count: 0);

              
            });

            if (dateNow - timesTamp < _rateLimitWindow)
            {
                if (count >= 8)
                {
                    return false;
                }

                _memoryCache.Set(cachKey, (timesTamp, count += 1), _rateLimitWindow);
            }
            else
            {
                _memoryCache.Set(cachKey, (timesTamp, count), _rateLimitWindow);

            }
            return true;



        }

        private void ApplySecurity(HttpContext context)
        {
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["X-Xss-Protection"] = "1,mode=block";
            context.Response.Headers["X-Frame-Options"] = "DENY";
        } 


    }
}
