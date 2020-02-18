using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class DecesionStatusRepository : RepositoryBase<DecesionStatus>, IDecesionStatusRepository
    {
        public DecesionStatusRepository(EPlastDBContext dbContext) : base(dbContext)
        {
        }
    }
}