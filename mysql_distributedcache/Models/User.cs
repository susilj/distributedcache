
namespace mysql_distributedcache.Models
{
    using System;

    public class User
    {
        public User()
        {
            Createdtime = DateTime.UtcNow;

            SubscriptionActive = 1;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }
        
        public string Email { get; set; }
        
        public string Passwordhash { get; set; }
        
        public DateTime Createdtime { get; set; }
        
        public sbyte Deleted { get; set; }

        public sbyte SubscriptionActive { get; set; }
    }
}
