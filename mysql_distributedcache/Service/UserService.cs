
namespace mysql_distributedcache.Service
{
    using Arch.EntityFrameworkCore.UnitOfWork;
    using Arch.EntityFrameworkCore.UnitOfWork.Collections;
    using mysql_distributedcache.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class UserService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IRepository<User> userRepo;

        public UserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            userRepo = this.unitOfWork.GetRepository<User>();
        }

        public async Task<User> AuthenticateUserAsync(string email, string password)
        {
            string encryptedPassword = password; // utilities.Encrypt(password);

            User user = await userRepo.GetFirstOrDefaultAsync(
                                            predicate: u => u.Email == email
                                                            && u.Passwordhash == encryptedPassword
                                                            && u.Deleted != 1);

            return user;
        }

        public async Task<bool> Insert(User user)
        {
            await userRepo.InsertAsync(user);

            int inserted = await unitOfWork.SaveChangesAsync();

            return inserted > 0;
        }

        public async Task<IList<User>> FetchAll()
        {
            int totalUsers = userRepo.Count();

            IPagedList<User> users = await userRepo.GetPagedListAsync<User>(
                                                        u => u,
                                                        disableTracking: true,
                                                        pageSize: totalUsers);

            return users.Items;
        }
    }
}
