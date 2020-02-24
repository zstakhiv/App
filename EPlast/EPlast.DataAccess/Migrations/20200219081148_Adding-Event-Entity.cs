using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddingEventEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
