using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decesions_Organs_OrganID",
                table: "Decesions");

            migrationBuilder.DropTable(
                name: "Organs");

            migrationBuilder.RenameColumn(
                name: "OrganID",
                table: "Decesions",
                newName: "OrganizationID");

            migrationBuilder.RenameIndex(
                name: "IX_Decesions_OrganID",
                table: "Decesions",
                newName: "IX_Decesions_OrganizationID");

            migrationBuilder.CreateTable(
                name: "Organization",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganizationName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.ID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Decesions_Organization_OrganizationID",
                table: "Decesions",
                column: "OrganizationID",
                principalTable: "Organization",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decesions_Organization_OrganizationID",
                table: "Decesions");

            migrationBuilder.DropTable(
                name: "Organization");

            migrationBuilder.RenameColumn(
                name: "OrganizationID",
                table: "Decesions",
                newName: "OrganID");

            migrationBuilder.RenameIndex(
                name: "IX_Decesions_OrganizationID",
                table: "Decesions",
                newName: "IX_Decesions_OrganID");

            migrationBuilder.CreateTable(
                name: "Organs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrganName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organs", x => x.ID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Decesions_Organs_OrganID",
                table: "Decesions",
                column: "OrganID",
                principalTable: "Organs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
