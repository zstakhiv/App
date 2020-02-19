using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class MigrationTableUserProfile2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "PhoneNUmber",
                table: "UserProfiles",
                newName: "PhoneNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "UserProfiles",
                newName: "PhoneNUmber");

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
    }
}
