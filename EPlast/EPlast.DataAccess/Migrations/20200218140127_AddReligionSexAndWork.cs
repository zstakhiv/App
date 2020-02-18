using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddReligionSexAndWork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReligionID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SexID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Religions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ReligionName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Religions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Sexes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SexName = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sexes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Works",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlaceOfwork = table.Column<string>(maxLength: 50, nullable: false),
                    Position = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Works", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ReligionID",
                table: "AspNetUsers",
                column: "ReligionID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SexID",
                table: "AspNetUsers",
                column: "SexID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_WorkID",
                table: "AspNetUsers",
                column: "WorkID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Religions_ReligionID",
                table: "AspNetUsers",
                column: "ReligionID",
                principalTable: "Religions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Sexes_SexID",
                table: "AspNetUsers",
                column: "SexID",
                principalTable: "Sexes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Works_WorkID",
                table: "AspNetUsers",
                column: "WorkID",
                principalTable: "Works",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Religions_ReligionID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Sexes_SexID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Works_WorkID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Religions");

            migrationBuilder.DropTable(
                name: "Sexes");

            migrationBuilder.DropTable(
                name: "Works");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ReligionID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SexID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_WorkID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ReligionID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SexID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WorkID",
                table: "AspNetUsers");
        }
    }
}
