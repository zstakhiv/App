using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Add_AnnualReportStatus_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnnualReportStatusId",
                table: "AnnualReports",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AnnualReportStatus",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnualReportStatus", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnualReports_AnnualReportStatusId",
                table: "AnnualReports",
                column: "AnnualReportStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualReports_AnnualReportStatus_AnnualReportStatusId",
                table: "AnnualReports",
                column: "AnnualReportStatusId",
                principalTable: "AnnualReportStatus",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnualReports_AnnualReportStatus_AnnualReportStatusId",
                table: "AnnualReports");

            migrationBuilder.DropTable(
                name: "AnnualReportStatus");

            migrationBuilder.DropIndex(
                name: "IX_AnnualReports_AnnualReportStatusId",
                table: "AnnualReports");

            migrationBuilder.DropColumn(
                name: "AnnualReportStatusId",
                table: "AnnualReports");
        }
    }
}
