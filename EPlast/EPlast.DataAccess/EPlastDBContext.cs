using EPlast.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace EPlast.DataAccess
{
    public class EPlastDBContext : DbContext
    {
        public EPlastDBContext(DbContextOptions<EPlastDBContext> options): base(options)
        {           
        }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
