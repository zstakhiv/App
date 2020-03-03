using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class SubEventCategoryRepository : RepositoryBase<SubEventCategory>, ISubEventCategoryRepository
    {
        public SubEventCategoryRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}