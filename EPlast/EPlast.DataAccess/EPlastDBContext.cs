using EPlast.DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EPlast.DataAccess
{
    public class EPlastDBContext : IdentityDbContext<User>
    {
        public EPlastDBContext(DbContextOptions<EPlastDBContext> options) : base(options)
        {
        }

        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<DocumentTemplate> DocumentTemplates { get; set; }
        public DbSet<Organ> Organs { get; set; }
        public DbSet<DecesionStatus> DecesionStatuses { get; set; }
        public DbSet<DecesionTarget> DecesionTargets { get; set; }
        public DbSet<Decesion> Decesions { get; set; }
    }
}