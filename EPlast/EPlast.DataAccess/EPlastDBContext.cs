﻿using EPlast.DataAccess.Entities;
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
        public DbSet<Religion> Religions { get; set; }
        public DbSet<Sex> Sexes { get; set; }
        public DbSet<Work> Works { get; set; }
    }
}