using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class SexRepository : RepositoryBase<Sex>, ISexRepository
    {
        public SexRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
