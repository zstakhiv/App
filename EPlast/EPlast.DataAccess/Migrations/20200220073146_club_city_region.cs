using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class club_city_region : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CityAdministration_City_CityID",
                table: "CityAdministration");

            migrationBuilder.DropForeignKey(
                name: "FK_CityAdministration_CityMembers_CityMembersID",
                table: "CityAdministration");

            migrationBuilder.DropForeignKey(
                name: "FK_CityDocuments_CityDocumentType_CityDocumentTypeID",
                table: "CityDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_CityDocuments_City_CityID",
                table: "CityDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_CityMembers_City_CityID",
                table: "CityMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CityDocumentType",
                table: "CityDocumentType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CityAdministration",
                table: "CityAdministration");

            migrationBuilder.DropPrimaryKey(
                name: "PK_City",
                table: "City");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "CityMembers");

            migrationBuilder.RenameTable(
                name: "CityDocumentType",
                newName: "CityDocumentTypes");

            migrationBuilder.RenameTable(
                name: "CityAdministration",
                newName: "CityAdministrations");

            migrationBuilder.RenameTable(
                name: "City",
                newName: "Cities");

            migrationBuilder.RenameIndex(
                name: "IX_CityAdministration_CityMembersID",
                table: "CityAdministrations",
                newName: "IX_CityAdministrations_CityMembersID");

            migrationBuilder.RenameIndex(
                name: "IX_CityAdministration_CityID",
                table: "CityAdministrations",
                newName: "IX_CityAdministrations_CityID");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "CityMembers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "CityMembers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "AdminTypeID",
                table: "CityAdministrations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionID",
                table: "Cities",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CityDocumentTypes",
                table: "CityDocumentTypes",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CityAdministrations",
                table: "CityAdministrations",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cities",
                table: "Cities",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "AdminTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdminTypeName = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Clubs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClubName = table.Column<string>(maxLength: 50, nullable: false),
                    ClubURL = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RegionName = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UnconfirmedCityMember",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    CityID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnconfirmedCityMember", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UnconfirmedCityMember_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnconfirmedCityMember_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClubMembers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    ClubID = table.Column<int>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubMembers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClubMembers_Clubs_ClubID",
                        column: x => x.ClubID,
                        principalTable: "Clubs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClubMembers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegionAdministrations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdminTypeID = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    RegionID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionAdministrations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RegionAdministrations_AdminTypes_AdminTypeID",
                        column: x => x.AdminTypeID,
                        principalTable: "AdminTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegionAdministrations_Regions_RegionID",
                        column: x => x.RegionID,
                        principalTable: "Regions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegionAdministrations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClubAdministrations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdminTypeID = table.Column<int>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    ClubID = table.Column<int>(nullable: true),
                    ClubMembersID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubAdministrations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClubAdministrations_AdminTypes_AdminTypeID",
                        column: x => x.AdminTypeID,
                        principalTable: "AdminTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClubAdministrations_Clubs_ClubID",
                        column: x => x.ClubID,
                        principalTable: "Clubs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClubAdministrations_ClubMembers_ClubMembersID",
                        column: x => x.ClubMembersID,
                        principalTable: "ClubMembers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CityAdministrations_AdminTypeID",
                table: "CityAdministrations",
                column: "AdminTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_RegionID",
                table: "Cities",
                column: "RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_ClubAdministrations_AdminTypeID",
                table: "ClubAdministrations",
                column: "AdminTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_ClubAdministrations_ClubID",
                table: "ClubAdministrations",
                column: "ClubID");

            migrationBuilder.CreateIndex(
                name: "IX_ClubAdministrations_ClubMembersID",
                table: "ClubAdministrations",
                column: "ClubMembersID");

            migrationBuilder.CreateIndex(
                name: "IX_ClubMembers_ClubID",
                table: "ClubMembers",
                column: "ClubID");

            migrationBuilder.CreateIndex(
                name: "IX_ClubMembers_UserId",
                table: "ClubMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RegionAdministrations_AdminTypeID",
                table: "RegionAdministrations",
                column: "AdminTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_RegionAdministrations_RegionID",
                table: "RegionAdministrations",
                column: "RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_RegionAdministrations_UserId",
                table: "RegionAdministrations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UnconfirmedCityMember_CityID",
                table: "UnconfirmedCityMember",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_UnconfirmedCityMember_UserId",
                table: "UnconfirmedCityMember",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Regions_RegionID",
                table: "Cities",
                column: "RegionID",
                principalTable: "Regions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CityAdministrations_AdminTypes_AdminTypeID",
                table: "CityAdministrations",
                column: "AdminTypeID",
                principalTable: "AdminTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CityAdministrations_Cities_CityID",
                table: "CityAdministrations",
                column: "CityID",
                principalTable: "Cities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CityAdministrations_CityMembers_CityMembersID",
                table: "CityAdministrations",
                column: "CityMembersID",
                principalTable: "CityMembers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CityDocuments_CityDocumentTypes_CityDocumentTypeID",
                table: "CityDocuments",
                column: "CityDocumentTypeID",
                principalTable: "CityDocumentTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CityDocuments_Cities_CityID",
                table: "CityDocuments",
                column: "CityID",
                principalTable: "Cities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CityMembers_Cities_CityID",
                table: "CityMembers",
                column: "CityID",
                principalTable: "Cities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Regions_RegionID",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_CityAdministrations_AdminTypes_AdminTypeID",
                table: "CityAdministrations");

            migrationBuilder.DropForeignKey(
                name: "FK_CityAdministrations_Cities_CityID",
                table: "CityAdministrations");

            migrationBuilder.DropForeignKey(
                name: "FK_CityAdministrations_CityMembers_CityMembersID",
                table: "CityAdministrations");

            migrationBuilder.DropForeignKey(
                name: "FK_CityDocuments_CityDocumentTypes_CityDocumentTypeID",
                table: "CityDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_CityDocuments_Cities_CityID",
                table: "CityDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_CityMembers_Cities_CityID",
                table: "CityMembers");

            migrationBuilder.DropTable(
                name: "ClubAdministrations");

            migrationBuilder.DropTable(
                name: "RegionAdministrations");

            migrationBuilder.DropTable(
                name: "UnconfirmedCityMember");

            migrationBuilder.DropTable(
                name: "ClubMembers");

            migrationBuilder.DropTable(
                name: "AdminTypes");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Clubs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CityDocumentTypes",
                table: "CityDocumentTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CityAdministrations",
                table: "CityAdministrations");

            migrationBuilder.DropIndex(
                name: "IX_CityAdministrations_AdminTypeID",
                table: "CityAdministrations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cities",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_RegionID",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "CityMembers");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "CityMembers");

            migrationBuilder.DropColumn(
                name: "AdminTypeID",
                table: "CityAdministrations");

            migrationBuilder.DropColumn(
                name: "RegionID",
                table: "Cities");

            migrationBuilder.RenameTable(
                name: "CityDocumentTypes",
                newName: "CityDocumentType");

            migrationBuilder.RenameTable(
                name: "CityAdministrations",
                newName: "CityAdministration");

            migrationBuilder.RenameTable(
                name: "Cities",
                newName: "City");

            migrationBuilder.RenameIndex(
                name: "IX_CityAdministrations_CityMembersID",
                table: "CityAdministration",
                newName: "IX_CityAdministration_CityMembersID");

            migrationBuilder.RenameIndex(
                name: "IX_CityAdministrations_CityID",
                table: "CityAdministration",
                newName: "IX_CityAdministration_CityID");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "CityMembers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CityDocumentType",
                table: "CityDocumentType",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CityAdministration",
                table: "CityAdministration",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_City",
                table: "City",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CityAdministration_City_CityID",
                table: "CityAdministration",
                column: "CityID",
                principalTable: "City",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CityAdministration_CityMembers_CityMembersID",
                table: "CityAdministration",
                column: "CityMembersID",
                principalTable: "CityMembers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CityDocuments_CityDocumentType_CityDocumentTypeID",
                table: "CityDocuments",
                column: "CityDocumentTypeID",
                principalTable: "CityDocumentType",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CityDocuments_City_CityID",
                table: "CityDocuments",
                column: "CityID",
                principalTable: "City",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CityMembers_City_CityID",
                table: "CityMembers",
                column: "CityID",
                principalTable: "City",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
