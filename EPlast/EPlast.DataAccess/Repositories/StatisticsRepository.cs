using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;

namespace EPlast.DataAccess.Repositories
{
    public class StatisticsRepository : RepositoryBase<Statistic>, IStatisticsRepository
    {
        public StatisticsRepository(EPlastDBContext dBContext)
            : base(dBContext)
        {

        }
    }
}