
namespace mysql_distributedcache.TokenHandler
{
    using System.Threading.Tasks;
    
    public interface ITokenManager
    {
        Task<bool> IsCurrentActiveToken();

        Task DeactivateCurrentAsync();

        Task<bool> IsActiveAsync(string token);

        Task DeactivateAsync(string token);

        Task CacheToken(string key, string value);
    }
}
