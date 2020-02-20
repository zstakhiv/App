using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Delete_unnecessary_fields_from_AnnualReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeneficiaryCount",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "BirdieSocketCount",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "NotNameCount",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "SocketsCount",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "SwarmsSavedCount",
                table: "AnnualReports");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BeneficiaryCount",
                table: "AnnualReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BirdieSocketCount",
                table: "AnnualReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NotNameCount",
                table: "AnnualReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SocketsCount",
                table: "AnnualReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SwarmsSavedCount",
                table: "AnnualReports",
                nullable: false,
                defaultValue: 0);
        }
    }
}
