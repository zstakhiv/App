using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class UserPlastDegreeTypesRepository : RepositoryBase<UserPlastDegreeType>, IUserPlastDegreeTypesRepository
    {
        public UserPlastDegreeTypesRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}