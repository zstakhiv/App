using EPlast.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Repositories
{
    public class ConfirmatorRepository : RepositoryBase<Confirmator>, IConfirmatorRepository
    {
        public ConfirmatorRepository(EPlastDBContext dbContext)
           : base(dbContext)
        {
        }
    }
}
