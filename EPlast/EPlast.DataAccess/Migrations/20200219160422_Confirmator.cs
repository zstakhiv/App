using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Confirmator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comissions");

            migrationBuilder.DropTable(
                name: "UsersComissions");

            migrationBuilder.CreateTable(
                name: "ConfirmedUsers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ConfirmDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfirmedUsers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ConfirmedUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Confirmators",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    ConfirmedUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Confirmators", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Confirmators_ConfirmedUsers_ConfirmedUserID",
                        column: x => x.ConfirmedUserID,
                        principalTable: "ConfirmedUsers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Confirmators_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Confirmators_ConfirmedUserID",
                table: "Confirmators",
                column: "ConfirmedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Confirmators_UserId",
                table: "Confirmators",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmedUsers_UserId",
                table: "ConfirmedUsers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Confirmators");

            migrationBuilder.DropTable(
                name: "ConfirmedUsers");

            migrationBuilder.CreateTable(
                name: "UsersComissions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ComissionDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersComissions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UsersComissions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comissions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserComissionID = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comissions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Comissions_UsersComissions_UserComissionID",
                        column: x => x.UserComissionID,
                        principalTable: "UsersComissions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comissions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comissions_UserComissionID",
                table: "Comissions",
                column: "UserComissionID");

            migrationBuilder.CreateIndex(
                name: "IX_Comissions_UserId",
                table: "Comissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersComissions_UserId",
                table: "UsersComissions",
                column: "UserId");
        }
    }
}
