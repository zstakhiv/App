using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddTablesNationalityEducationDegree : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Nationalities",
                newName: "NationalityName");

            migrationBuilder.CreateTable(
                name: "Degrees",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DegreeName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Degrees", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlaceOfStudy = table.Column<string>(maxLength: 50, nullable: false),
                    Speciality = table.Column<string>(maxLength: 50, nullable: false),
                    DegreeID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Educations_Degrees_DegreeID",
                        column: x => x.DegreeID,
                        principalTable: "Degrees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Educations_DegreeID",
                table: "Educations",
                column: "DegreeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Educations");

            migrationBuilder.DropTable(
                name: "Degrees");

            migrationBuilder.RenameColumn(
                name: "NationalityName",
                table: "Nationalities",
                newName: "Name");
        }
    }
}
