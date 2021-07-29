
namespace mysql_distributedcache.TokenHandler
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Primitives;
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TokenManager : ITokenManager
    {
        private readonly IDistributedCache cache;

        private readonly IHttpContextAccessor httpContextAccessor;

        public TokenManager(
            IDistributedCache cache,
            IHttpContextAccessor httpContextAccessor)
        {
            this.cache = cache;

            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task CacheToken(string key, string value)
        {
            await cache.SetAsync(key, Encoding.ASCII.GetBytes(value), new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(1),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(360)
            });
        }

        public async Task DeactivateAsync(string token)
            => await cache.SetStringAsync(GetKey(token),
                    " ", new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2)
                    });

        public async Task DeactivateCurrentAsync()
            => await DeactivateAsync(GetCurrentAsync());

        public async Task<bool> IsActiveAsync(string token)
        {
            var key = GetKey(token);

            var tokenvalue = await cache.GetStringAsync(key);
            
            return tokenvalue == null;
        }

        public async Task<bool> IsCurrentActiveToken()
            => await IsActiveAsync(GetCurrentAsync());

        private string GetCurrentAsync()
        {
            var authorizationHeader = httpContextAccessor.HttpContext.Request.Headers["authorization"];

            return authorizationHeader == StringValues.Empty
                                            ? string.Empty
                                            : authorizationHeader.Single().Split(" ").Last();
        }

        private static string GetKey(string token)
            => $"tokens:{token}:deactivated";
    }
}
