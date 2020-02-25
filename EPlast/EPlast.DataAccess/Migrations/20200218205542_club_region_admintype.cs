using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class club_region_admintype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 16, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    CityURL = table.Column<string>(maxLength: 256, nullable: true),
                    Description = table.Column<string>(maxLength: 1024, nullable: true),
                    Street = table.Column<string>(maxLength: 60, nullable: false),
                    HouseNumber = table.Column<string>(maxLength: 10, nullable: false),
                    OfficeNumber = table.Column<string>(maxLength: 10, nullable: true),
                    PostIndex = table.Column<string>(maxLength: 7, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CityDocumentType",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityDocumentType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CityMembers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    CityID = table.Column<int>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityMembers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CityMembers_City_CityID",
                        column: x => x.CityID,
                        principalTable: "City",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CityMembers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CityDocuments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubmitDate = table.Column<DateTime>(nullable: true),
                    DocumentURL = table.Column<string>(maxLength: 256, nullable: false),
                    CityDocumentTypeID = table.Column<int>(nullable: true),
                    CityID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityDocuments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CityDocuments_CityDocumentType_CityDocumentTypeID",
                        column: x => x.CityDocumentTypeID,
                        principalTable: "CityDocumentType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CityDocuments_City_CityID",
                        column: x => x.CityID,
                        principalTable: "City",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CityAdministration",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    CityID = table.Column<int>(nullable: true),
                    CityMembersID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityAdministration", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CityAdministration_City_CityID",
                        column: x => x.CityID,
                        principalTable: "City",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CityAdministration_CityMembers_CityMembersID",
                        column: x => x.CityMembersID,
                        principalTable: "CityMembers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CityAdministration_CityID",
                table: "CityAdministration",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_CityAdministration_CityMembersID",
                table: "CityAdministration",
                column: "CityMembersID");

            migrationBuilder.CreateIndex(
                name: "IX_CityDocuments_CityDocumentTypeID",
                table: "CityDocuments",
                column: "CityDocumentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CityDocuments_CityID",
                table: "CityDocuments",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_CityMembers_CityID",
                table: "CityMembers",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_CityMembers_UserId",
                table: "CityMembers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CityAdministration");

            migrationBuilder.DropTable(
                name: "CityDocuments");

            migrationBuilder.DropTable(
                name: "CityMembers");

            migrationBuilder.DropTable(
                name: "CityDocumentType");

            migrationBuilder.DropTable(
                name: "City");
        }
    }
}
