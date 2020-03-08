using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class CityLegalStatusTypeRepository : RepositoryBase<CityLegalStatusType>, ICityLegalStatusTypesRepository 
    {
        public CityLegalStatusTypeRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}