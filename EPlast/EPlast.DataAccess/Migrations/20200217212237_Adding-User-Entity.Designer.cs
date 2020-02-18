﻿// <auto-generated />
using System;
using EPlast.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EPlast.DataAccess.Migrations
{
    [DbContext(typeof(EPlastDBContext))]
    [Migration("20200217212237_Adding-User-Entity")]
    partial class AddingUserEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EPlast.DataAccess.Entities.Nationality", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.ToTable("Nationalities");
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int?>("NationalityID");

                    b.HasKey("ID");

                    b.HasIndex("NationalityID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.User", b =>
                {
                    b.HasOne("EPlast.DataAccess.Entities.Nationality", "Nationality")
                        .WithMany("Users")
                        .HasForeignKey("NationalityID");
                });
#pragma warning restore 612, 618
        }
    }
}
