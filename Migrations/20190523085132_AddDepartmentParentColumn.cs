using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFrameworkConsole.Migrations
{
    public partial class AddDepartmentParentColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ParentId",
                table: "Departments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ParentId",
                table: "Departments",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Departments_ParentId",
                table: "Departments",
                column: "ParentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Departments_ParentId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_ParentId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Departments");
        }
    }
}
