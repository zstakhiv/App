using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EPlast.DataAccess.Migrations
{
    public partial class RenameToGenderAndRenameToApprover : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConfirmedUser_Confirmator_ConfirmatorID",
                table: "ConfirmedUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ConfirmedUser_AspNetUsers_UserId",
                table: "ConfirmedUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Decesions_Organs_OrganID",
                table: "Decesions");

            migrationBuilder.DropForeignKey(
                name: "FK_Education_Degree_DegreeID",
                table: "Education");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Education_EducationID",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Religion_ReligionID",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Sex_SexID",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Work_WorkID",
                table: "UserProfiles");

            migrationBuilder.DropTable(
                name: "Confirmator");

            migrationBuilder.DropTable(
                name: "Organs");

            migrationBuilder.DropTable(
                name: "Sex");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Work",
                table: "Work");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Religion",
                table: "Religion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Education",
                table: "Education");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Degree",
                table: "Degree");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConfirmedUser",
                table: "ConfirmedUser");

            migrationBuilder.DropIndex(
                name: "IX_ConfirmedUser_ConfirmatorID",
                table: "ConfirmedUser");

            migrationBuilder.RenameTable(
                name: "Work",
                newName: "Works");

            migrationBuilder.RenameTable(
                name: "Religion",
                newName: "Religions");

            migrationBuilder.RenameTable(
                name: "Education",
                newName: "Educations");

            migrationBuilder.RenameTable(
                name: "Degree",
                newName: "Degrees");

            migrationBuilder.RenameTable(
                name: "ConfirmedUser",
                newName: "ConfirmedUsers");

            migrationBuilder.RenameColumn(
                name: "SexID",
                table: "UserProfiles",
                newName: "GenderID");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfiles_SexID",
                table: "UserProfiles",
                newName: "IX_UserProfiles_GenderID");

            migrationBuilder.RenameColumn(
                name: "OrganID",
                table: "Decesions",
                newName: "OrganizationID");

            migrationBuilder.RenameIndex(
                name: "IX_Decesions_OrganID",
                table: "Decesions",
                newName: "IX_Decesions_OrganizationID");

            migrationBuilder.RenameIndex(
                name: "IX_Education_DegreeID",
                table: "Educations",
                newName: "IX_Educations_DegreeID");

            migrationBuilder.RenameColumn(
                name: "ConfirmatorID",
                table: "ConfirmedUsers",
                newName: "ApproverID");

            migrationBuilder.RenameIndex(
                name: "IX_ConfirmedUser_UserId",
                table: "ConfirmedUsers",
                newName: "IX_ConfirmedUsers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Works",
                table: "Works",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Religions",
                table: "Religions",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Educations",
                table: "Educations",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Degrees",
                table: "Degrees",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConfirmedUsers",
                table: "ConfirmedUsers",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "Approvers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Approvers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Approvers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GenderName = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.ID);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmedUsers_ApproverID",
                table: "ConfirmedUsers",
                column: "ApproverID",
                unique: true,
                filter: "[ApproverID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Approvers_UserId",
                table: "Approvers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfirmedUsers_Approvers_ApproverID",
                table: "ConfirmedUsers",
                column: "ApproverID",
                principalTable: "Approvers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfirmedUsers_AspNetUsers_UserId",
                table: "ConfirmedUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Decesions_Organization_OrganizationID",
                table: "Decesions",
                column: "OrganizationID",
                principalTable: "Organization",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Educations_Degrees_DegreeID",
                table: "Educations",
                column: "DegreeID",
                principalTable: "Degrees",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Educations_EducationID",
                table: "UserProfiles",
                column: "EducationID",
                principalTable: "Educations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Genders_GenderID",
                table: "UserProfiles",
                column: "GenderID",
                principalTable: "Genders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Religions_ReligionID",
                table: "UserProfiles",
                column: "ReligionID",
                principalTable: "Religions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Works_WorkID",
                table: "UserProfiles",
                column: "WorkID",
                principalTable: "Works",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConfirmedUsers_Approvers_ApproverID",
                table: "ConfirmedUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ConfirmedUsers_AspNetUsers_UserId",
                table: "ConfirmedUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Decesions_Organization_OrganizationID",
                table: "Decesions");

            migrationBuilder.DropForeignKey(
                name: "FK_Educations_Degrees_DegreeID",
                table: "Educations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Educations_EducationID",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Genders_GenderID",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Religions_ReligionID",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Works_WorkID",
                table: "UserProfiles");

            migrationBuilder.DropTable(
                name: "Approvers");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "Organization");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Works",
                table: "Works");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Religions",
                table: "Religions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Educations",
                table: "Educations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Degrees",
                table: "Degrees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConfirmedUsers",
                table: "ConfirmedUsers");

            migrationBuilder.DropIndex(
                name: "IX_ConfirmedUsers_ApproverID",
                table: "ConfirmedUsers");

            migrationBuilder.RenameTable(
                name: "Works",
                newName: "Work");

            migrationBuilder.RenameTable(
                name: "Religions",
                newName: "Religion");

            migrationBuilder.RenameTable(
                name: "Educations",
                newName: "Education");

            migrationBuilder.RenameTable(
                name: "Degrees",
                newName: "Degree");

            migrationBuilder.RenameTable(
                name: "ConfirmedUsers",
                newName: "ConfirmedUser");

            migrationBuilder.RenameColumn(
                name: "GenderID",
                table: "UserProfiles",
                newName: "SexID");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfiles_GenderID",
                table: "UserProfiles",
                newName: "IX_UserProfiles_SexID");

            migrationBuilder.RenameColumn(
                name: "OrganizationID",
                table: "Decesions",
                newName: "OrganID");

            migrationBuilder.RenameIndex(
                name: "IX_Decesions_OrganizationID",
                table: "Decesions",
                newName: "IX_Decesions_OrganID");

            migrationBuilder.RenameIndex(
                name: "IX_Educations_DegreeID",
                table: "Education",
                newName: "IX_Education_DegreeID");

            migrationBuilder.RenameColumn(
                name: "ApproverID",
                table: "ConfirmedUser",
                newName: "ConfirmatorID");

            migrationBuilder.RenameIndex(
                name: "IX_ConfirmedUsers_UserId",
                table: "ConfirmedUser",
                newName: "IX_ConfirmedUser_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Work",
                table: "Work",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Religion",
                table: "Religion",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Education",
                table: "Education",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Degree",
                table: "Degree",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConfirmedUser",
                table: "ConfirmedUser",
                column: "ID");

            migrationBuilder.CreateTable(
                name: "Confirmator",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Confirmator", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Confirmator_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "Sex",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SexName = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sex", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmedUser_ConfirmatorID",
                table: "ConfirmedUser",
                column: "ConfirmatorID",
                unique: true,
                filter: "[ConfirmatorID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Confirmator_UserId",
                table: "Confirmator",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfirmedUser_Confirmator_ConfirmatorID",
                table: "ConfirmedUser",
                column: "ConfirmatorID",
                principalTable: "Confirmator",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConfirmedUser_AspNetUsers_UserId",
                table: "ConfirmedUser",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Decesions_Organs_OrganID",
                table: "Decesions",
                column: "OrganID",
                principalTable: "Organs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Education_Degree_DegreeID",
                table: "Education",
                column: "DegreeID",
                principalTable: "Degree",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Education_EducationID",
                table: "UserProfiles",
                column: "EducationID",
                principalTable: "Education",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Religion_ReligionID",
                table: "UserProfiles",
                column: "ReligionID",
                principalTable: "Religion",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Sex_SexID",
                table: "UserProfiles",
                column: "SexID",
                principalTable: "Sex",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Work_WorkID",
                table: "UserProfiles",
                column: "WorkID",
                principalTable: "Work",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
