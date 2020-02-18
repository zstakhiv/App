using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
