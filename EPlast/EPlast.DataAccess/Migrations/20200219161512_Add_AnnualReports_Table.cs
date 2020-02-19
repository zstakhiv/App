using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Add_AnnualReports_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnnualReports",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    SocketsCount = table.Column<int>(nullable: false),
                    SwarmsSavedCount = table.Column<int>(nullable: false),
                    NotNameCount = table.Column<int>(nullable: false),
                    BirdieSocketCount = table.Column<int>(nullable: false),
                    BeneficiaryCount = table.Column<int>(nullable: false),
                    ImprovementNeeds = table.Column<string>(maxLength: 500, nullable: false),
                    GovernmentFunds = table.Column<int>(nullable: false),
                    ContributionFunds = table.Column<int>(nullable: false),
                    PlastFunds = table.Column<int>(nullable: false),
                    SponsorFunds = table.Column<int>(nullable: false),
                    PropertyList = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnualReports", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnualReports");
        }
    }
}
