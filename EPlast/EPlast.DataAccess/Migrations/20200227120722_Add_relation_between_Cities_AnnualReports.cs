using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Add_relation_between_Cities_AnnualReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "AnnualReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AnnualReports_CityId",
                table: "AnnualReports",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualReports_Cities_CityId",
                table: "AnnualReports",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnualReports_Cities_CityId",
                table: "AnnualReports");

            migrationBuilder.DropIndex(
                name: "IX_AnnualReports_CityId",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "AnnualReports");
        }
    }
}
