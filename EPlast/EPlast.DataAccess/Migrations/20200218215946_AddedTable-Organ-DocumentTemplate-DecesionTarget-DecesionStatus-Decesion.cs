using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class AddedTableOrganDocumentTemplateDecesionTargetDecesionStatusDecesion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Decesions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DecesionStatusID",
                table: "Decesions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DecesionTargetID",
                table: "Decesions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Decesions",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OrganID",
                table: "Decesions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Decesions_DecesionStatusID",
                table: "Decesions",
                column: "DecesionStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Decesions_DecesionTargetID",
                table: "Decesions",
                column: "DecesionTargetID");

            migrationBuilder.CreateIndex(
                name: "IX_Decesions_OrganID",
                table: "Decesions",
                column: "OrganID");

            migrationBuilder.AddForeignKey(
                name: "FK_Decesions_DecesionStatuses_DecesionStatusID",
                table: "Decesions",
                column: "DecesionStatusID",
                principalTable: "DecesionStatuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Decesions_DecesionTargets_DecesionTargetID",
                table: "Decesions",
                column: "DecesionTargetID",
                principalTable: "DecesionTargets",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Decesions_Organs_OrganID",
                table: "Decesions",
                column: "OrganID",
                principalTable: "Organs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decesions_DecesionStatuses_DecesionStatusID",
                table: "Decesions");

            migrationBuilder.DropForeignKey(
                name: "FK_Decesions_DecesionTargets_DecesionTargetID",
                table: "Decesions");

            migrationBuilder.DropForeignKey(
                name: "FK_Decesions_Organs_OrganID",
                table: "Decesions");

            migrationBuilder.DropIndex(
                name: "IX_Decesions_DecesionStatusID",
                table: "Decesions");

            migrationBuilder.DropIndex(
                name: "IX_Decesions_DecesionTargetID",
                table: "Decesions");

            migrationBuilder.DropIndex(
                name: "IX_Decesions_OrganID",
                table: "Decesions");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Decesions");

            migrationBuilder.DropColumn(
                name: "DecesionStatusID",
                table: "Decesions");

            migrationBuilder.DropColumn(
                name: "DecesionTargetID",
                table: "Decesions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Decesions");

            migrationBuilder.DropColumn(
                name: "OrganID",
                table: "Decesions");
        }
    }
}
