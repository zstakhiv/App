using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class Migration10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Nationalities_NationalityID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EducationID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_NationalityID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EducationID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NationalityID",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EducationID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NationalityID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EducationID",
                table: "AspNetUsers",
                column: "EducationID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_NationalityID",
                table: "AspNetUsers",
                column: "NationalityID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Nationalities_NationalityID",
                table: "AspNetUsers",
                column: "NationalityID",
                principalTable: "Nationalities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
