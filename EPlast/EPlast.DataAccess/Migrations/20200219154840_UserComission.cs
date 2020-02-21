using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class UserComission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UsersComissions_UserComissionID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserComissionID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserConfignerID",
                table: "UsersComissions");

            migrationBuilder.DropColumn(
                name: "UserComissionID",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "UsersComissions",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UsersComissions",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "Comissions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    UserComissionID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comissions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Comissions_UsersComissions_UserComissionID",
                        column: x => x.UserComissionID,
                        principalTable: "UsersComissions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comissions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersComissions_UserId",
                table: "UsersComissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comissions_UserComissionID",
                table: "Comissions",
                column: "UserComissionID");

            migrationBuilder.CreateIndex(
                name: "IX_Comissions_UserId",
                table: "Comissions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersComissions_AspNetUsers_UserId",
                table: "UsersComissions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersComissions_AspNetUsers_UserId",
                table: "UsersComissions");

            migrationBuilder.DropTable(
                name: "Comissions");

            migrationBuilder.DropIndex(
                name: "IX_UsersComissions_UserId",
                table: "UsersComissions");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UsersComissions",
                newName: "UserID");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "UsersComissions",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "UserConfignerID",
                table: "UsersComissions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserComissionID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserComissionID",
                table: "AspNetUsers",
                column: "UserComissionID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UsersComissions_UserComissionID",
                table: "AspNetUsers",
                column: "UserComissionID",
                principalTable: "UsersComissions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
