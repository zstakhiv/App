using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Addingeventgallaryentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gallarys_Events_EventID",
                table: "Gallarys");

            migrationBuilder.DropIndex(
                name: "IX_Gallarys_EventID",
                table: "Gallarys");

            migrationBuilder.DropColumn(
                name: "EventID",
                table: "Gallarys");

            migrationBuilder.CreateTable(
                name: "EventGallarys",
                columns: table => new
                {
                    EventID = table.Column<int>(nullable: false),
                    GallaryID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventGallarys", x => new { x.EventID, x.GallaryID });
                    table.ForeignKey(
                        name: "FK_EventGallarys_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventGallarys_Gallarys_GallaryID",
                        column: x => x.GallaryID,
                        principalTable: "Gallarys",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventGallarys_GallaryID",
                table: "EventGallarys",
                column: "GallaryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventGallarys");

            migrationBuilder.AddColumn<int>(
                name: "EventID",
                table: "Gallarys",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gallarys_EventID",
                table: "Gallarys",
                column: "EventID");

            migrationBuilder.AddForeignKey(
                name: "FK_Gallarys_Events_EventID",
                table: "Gallarys",
                column: "EventID",
                principalTable: "Events",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
