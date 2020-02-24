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
    [Migration("20200219141101_Adding-Final-participant-entity")]
    partial class AddingFinalparticipantentity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EPlast.DataAccess.Entities.Event", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int>("EventCategoryID");

                    b.Property<DateTime>("EventDateEnd");

                    b.Property<DateTime>("EventDateStart");

                    b.Property<string>("EventName")
                        .IsRequired();

                    b.Property<int>("EventStatusID");

                    b.Property<string>("Eventlocation")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("EventCategoryID");

                    b.HasIndex("EventStatusID");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.EventAdmin", b =>
                {
                    b.Property<int>("EventID");

                    b.Property<string>("UserID");

                    b.HasKey("EventID", "UserID");

                    b.HasIndex("UserID");

                    b.ToTable("EventAdmin");
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.EventCategory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EventCategoryName")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("EventCategories");
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.EventGallary", b =>
                {
                    b.Property<int>("EventID");

                    b.Property<int>("GallaryID");

                    b.HasKey("EventID", "GallaryID");

                    b.HasIndex("GallaryID");

                    b.ToTable("EventGallarys");
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.EventStatus", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EventStatusName")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("EventStatuses");
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.Gallary", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("GalaryFileName")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Gallarys");
                });

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

            modelBuilder.Entity("EPlast.DataAccess.Entities.Participant", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EventId");

                    b.Property<int>("ParticipantStatusId");

                    b.Property<string>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("EventId");

                    b.HasIndex("ParticipantStatusId");

                    b.HasIndex("UserId");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.ParticipantStatus", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("UserEventStatusName")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("ParticipantStatuses");
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.SubEventCategory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EventCategoryID");

                    b.Property<string>("SubEventCategoryName")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("EventCategoryID");

                    b.ToTable("SubEventCategories");
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

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<int?>("NationalityID");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NationalityID");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
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

            modelBuilder.Entity("EPlast.DataAccess.Entities.Event", b =>
                {
                    b.HasOne("EPlast.DataAccess.Entities.EventCategory", "EventCategory")
                        .WithMany("Events")
                        .HasForeignKey("EventCategoryID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EPlast.DataAccess.Entities.EventStatus", "EventStatus")
                        .WithMany("Events")
                        .HasForeignKey("EventStatusID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.EventAdmin", b =>
                {
                    b.HasOne("EPlast.DataAccess.Entities.Event", "Event")
                        .WithMany("EventAdmins")
                        .HasForeignKey("EventID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EPlast.DataAccess.Entities.User", "User")
                        .WithMany("Events")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.EventGallary", b =>
                {
                    b.HasOne("EPlast.DataAccess.Entities.Event", "Event")
                        .WithMany("EventGallarys")
                        .HasForeignKey("EventID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EPlast.DataAccess.Entities.Gallary", "Gallary")
                        .WithMany("Events")
                        .HasForeignKey("GallaryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.Participant", b =>
                {
                    b.HasOne("EPlast.DataAccess.Entities.Event", "Event")
                        .WithMany("Participants")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EPlast.DataAccess.Entities.ParticipantStatus", "ParticipantStatus")
                        .WithMany("Participants")
                        .HasForeignKey("ParticipantStatusId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EPlast.DataAccess.Entities.User", "User")
                        .WithMany("Participants")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.SubEventCategory", b =>
                {
                    b.HasOne("EPlast.DataAccess.Entities.EventCategory", "EventCategory")
                        .WithMany("SubEventCategories")
                        .HasForeignKey("EventCategoryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EPlast.DataAccess.Entities.User", b =>
                {
                    b.HasOne("EPlast.DataAccess.Entities.Nationality", "Nationality")
                        .WithMany("Users")
                        .HasForeignKey("NationalityID");
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
