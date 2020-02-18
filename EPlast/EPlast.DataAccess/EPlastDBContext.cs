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
        public DbSet<CityMembers> CityMembers { get; set; }
    }
}
