using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Repositories
{
    public class UserProfileRepository: RepositoryBase<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
    }
}
