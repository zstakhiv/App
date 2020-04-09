using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace EPlast.DataAccess.Repositories
{
    public class ClubRepository : RepositoryBase<Club>, IClubRepository
    {
        public ClubRepository(EPlastDBContext dbContext)
            : base(dbContext)
        {
        }
            public new void Update(Club item)
        {
            var club = EPlastDBContext.Clubs.Find(item.ID);
            club.ClubName = item.ClubName;
            club.ClubURL = item.ClubURL;
            club.Description = item.Description;
            EPlastDBContext.Clubs.Update(club);
        }
    
    }
}
