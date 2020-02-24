using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class AnnualReportRepository : RepositoryBase<AnnualReport>, IAnnualReportRepository
    {
        public AnnualReportRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}