using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class ChangeConnectionEventcategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubEventCategories_EventCategories_EventCategoryID",
                table: "SubEventCategories");

            migrationBuilder.DropIndex(
                name: "IX_SubEventCategories_EventCategoryID",
                table: "SubEventCategories");

            migrationBuilder.AddColumn<int>(
                name: "SubEventCategoriesID",
                table: "EventCategories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventCategories_SubEventCategoriesID",
                table: "EventCategories",
                column: "SubEventCategoriesID");

            migrationBuilder.AddForeignKey(
                name: "FK_EventCategories_SubEventCategories_SubEventCategoriesID",
                table: "EventCategories",
                column: "SubEventCategoriesID",
                principalTable: "SubEventCategories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventCategories_SubEventCategories_SubEventCategoriesID",
                table: "EventCategories");

            migrationBuilder.DropIndex(
                name: "IX_EventCategories_SubEventCategoriesID",
                table: "EventCategories");

            migrationBuilder.DropColumn(
                name: "SubEventCategoriesID",
                table: "EventCategories");

            migrationBuilder.CreateIndex(
                name: "IX_SubEventCategories_EventCategoryID",
                table: "SubEventCategories",
                column: "EventCategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_SubEventCategories_EventCategories_EventCategoryID",
                table: "SubEventCategories",
                column: "EventCategoryID",
                principalTable: "EventCategories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
