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
    [Migration("20200219154840_UserComission")]
    partial class UserComission
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EPlast.DataAccess.Entities.Comission", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("UserComissionID");

                    b.Property<string>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("UserComissionID");

                    b.HasIndex("UserId");

                    b.ToTable("Comissions");
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.Degree", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DegreeName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.ToTable("Degrees");
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.Education", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("DegreeID");

                    b.Property<string>("PlaceOfStudy")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Speciality")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.HasIndex("DegreeID");

                    b.ToTable("Educations");
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.Nationality", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("NationalityName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.ToTable("Nationalities");
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.Religion", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ReligionName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.ToTable("Religions");
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.Sex", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("SexName")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.HasKey("ID");

                    b.ToTable("Sexes");
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FatherName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<DateTime>("RegistredOn");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<int>("UserProfileID");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("UserProfileID")
                        .IsUnique();

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.UserComission", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ComissionDate");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("UserId");

                    b.ToTable("UsersComissions");
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.UserProfile", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<DateTime>("DateTime");

                    b.Property<int?>("EducationID");

                    b.Property<int?>("NationalityID");

                    b.Property<int>("PhoneNumber");

                    b.Property<int?>("ReligionID");

                    b.Property<int?>("SexID");

                    b.Property<int>("UserID");

                    b.Property<int?>("WorkID");

                    b.HasKey("ID");

                    b.HasIndex("EducationID");

                    b.HasIndex("NationalityID");

                    b.HasIndex("ReligionID");

                    b.HasIndex("SexID");

                    b.HasIndex("WorkID");

                    b.ToTable("UserProfiles");
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.Work", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PlaceOfwork")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.ToTable("Works");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.Comission", b =>
                {
                    b.HasOne("EPlast.DataAccess.Entities.UserComission", "UserComission")
                        .WithMany("Comissions")
                        .HasForeignKey("UserComissionID");

                    b.HasOne("EPlast.DataAccess.Entities.User", "User")
                        .WithMany("Comissions")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.Education", b =>
                {
                    b.HasOne("EPlast.DataAccess.Entities.Degree", "Degree")
                        .WithMany("Educations")
                        .HasForeignKey("DegreeID");
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.User", b =>
                {
                    b.HasOne("EPlast.DataAccess.Entities.UserProfile", "UserProfile")
                        .WithOne("User")
                        .HasForeignKey("EPlast.DataAccess.Entities.User", "UserProfileID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.UserComission", b =>
                {
                    b.HasOne("EPlast.DataAccess.Entities.User", "User")
                        .WithMany("UserComissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.UserProfile", b =>
                {
                    b.HasOne("EPlast.DataAccess.Entities.Education", "Education")
                        .WithMany("UsersProfiles")
                        .HasForeignKey("EducationID");

                    b.HasOne("EPlast.DataAccess.Entities.Nationality", "Nationality")
                        .WithMany("UserProfiles")
                        .HasForeignKey("NationalityID");

                    b.HasOne("EPlast.DataAccess.Entities.Religion", "Religion")
                        .WithMany("UserProfiles")
                        .HasForeignKey("ReligionID");

                    b.HasOne("EPlast.DataAccess.Entities.Sex", "Sex")
                        .WithMany("UserProfiles")
                        .HasForeignKey("SexID");

                    b.HasOne("EPlast.DataAccess.Entities.Work", "Work")
                        .WithMany("UserProfiles")
                        .HasForeignKey("WorkID");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("EPlast.DataAccess.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("EPlast.DataAccess.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EPlast.DataAccess.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("EPlast.DataAccess.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
