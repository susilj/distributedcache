
namespace mysql_distributedcache.Middleware
{
    using Microsoft.AspNetCore.Http;
    using mysql_distributedcache.TokenHandler;
    using System.Net;
    using System.Threading.Tasks;
  
    public class TokenManagerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ITokenManager _tokenManager;

        public TokenManagerMiddleware(
            RequestDelegate next,
            ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;

            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (await _tokenManager.IsCurrentActiveToken())
            {
                await _next(context);

                return;
            }
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}
