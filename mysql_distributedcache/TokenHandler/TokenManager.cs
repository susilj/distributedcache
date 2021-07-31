
namespace mysql_distributedcache.TokenHandler
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Caching.Distributed;
    using System;
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

        public async Task DeactivateAsync(string token)
        {
            await cache.SetStringAsync(GetKey(token),
                    " ", new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)
                    });
        }

        public async Task DeactivateCurrentAsync()
            => await DeactivateAsync(GetCurrentAsync());

        public async Task<bool> IsActiveAsync(string token)
        {
            string key = GetKey(token);

            string tokenvalue = await cache.GetStringAsync(key);

            return tokenvalue == null;
        }

        public async Task<bool> IsCurrentActiveToken()
            => await IsActiveAsync(GetCurrentAsync());

        private string GetCurrentAsync()
        {
            httpContextAccessor.HttpContext.Items.TryGetValue("userId", out object userId);

            if (userId == null)
                return string.Empty;

            return userId.ToString();
        }

        private static string GetKey(string token)
            => $"tokens{token}deactivated";
    }
}
