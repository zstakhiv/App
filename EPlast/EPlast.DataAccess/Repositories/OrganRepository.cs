using EPlast.DataAccess.Entities;

namespace EPlast.DataAccess.Repositories
{
    public class OrganRepository : RepositoryBase<Organ>, IOrgranRepository
    {
        public OrganRepository(EPlastDBContext dbContext) : base(dbContext)
        {
        }
    }
}