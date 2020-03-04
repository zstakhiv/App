using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class AnnualReportStatusesRepository : RepositoryBase<AnnualReportStatus>, IAnnualReportStatusesRepository
    {
        public AnnualReportStatusesRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}