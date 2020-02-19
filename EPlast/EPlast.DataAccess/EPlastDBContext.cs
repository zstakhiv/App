using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EPlast.DataAccess
{
    public class EPlastDBContext : IdentityDbContext<User>
    {
        public EPlastDBContext(DbContextOptions<EPlastDBContext> options): base(options)
        {           
        }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<CityAdministration> CityAdministrations { get; set; }
        public DbSet<CityDocuments> CityDocuments { get; set; }
        public DbSet<CityDocumentType> CityDocumentTypes { get; set; }
        public DbSet<AdminType> AdminTypes  { get; set; }
        public DbSet<ClubMembers> ClubMembers { get; set; }
        public DbSet<ClubAdministration> ClubAdministrations { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<RegionAdministration> RegionAdministrations { get; set; }

    }
}
