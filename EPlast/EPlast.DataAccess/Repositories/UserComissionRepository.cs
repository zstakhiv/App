using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class UserComissionRepository : RepositoryBase<UserComission>, IUserComissionRepository
    {
        public UserComissionRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
