
namespace mysql_distributedcache.Data
{
    using Microsoft.EntityFrameworkCore;
    using mysql_distributedcache.Models;

    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected ApplicationContext(DbContextOptions options)
        : base(options)
        {
        }

        public virtual DbSet<User> User { get; set; }
    }
}
