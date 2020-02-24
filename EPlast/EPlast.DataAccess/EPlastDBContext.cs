using System;
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
       

        public DbSet<User> Users { get; set;}
        public DbSet<UserProfile> UserProfiles {get; set;}
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Gallary> Gallarys { get; set; }
        public DbSet<EventGallary> EventGallarys { get; set; }
        public DbSet<ParticipantStatus> ParticipantStatuses { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<SubEventCategory> SubEventCategories { get; set; }
        public DbSet<EventStatus> EventStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Event>()
                .HasKey(x => x.ID);

            modelBuilder.Entity<Gallary>()
                .HasKey(x => x.ID);

            modelBuilder.Entity<EventGallary>()
                .HasKey(x => new { x.EventID, x.GallaryID });
            modelBuilder.Entity<EventGallary>()
                .HasOne(x => x.Event)
                .WithMany(m => m.EventGallarys)
                .HasForeignKey(x => x.EventID);
            modelBuilder.Entity<EventGallary>()
                .HasOne(x => x.Gallary)
                .WithMany(e => e.Events)
                .HasForeignKey(x => x.GallaryID);

            modelBuilder.Entity<Event>()
                .HasKey(x => x.ID);

            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<EventAdmin>()
                .HasKey(x => new { x.EventID, x.UserID });
            modelBuilder.Entity<EventAdmin>()
                .HasOne(x => x.Event)
                .WithMany(m => m.EventAdmins)
                .HasForeignKey(x => x.EventID);
            modelBuilder.Entity<EventAdmin>()
                .HasOne(x => x.User)
                .WithMany(e => e.Events)
                .HasForeignKey(x => x.UserID);
        }

    }
}
