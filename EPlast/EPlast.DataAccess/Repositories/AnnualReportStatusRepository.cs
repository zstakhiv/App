using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class AnnualReportStatusRepository : RepositoryBase<AnnualReportStatus>, IAnnualReportStatusRepository
    {
        public AnnualReportStatusRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}