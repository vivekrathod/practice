using Microsoft.EntityFrameworkCore.Migrations;

namespace AzureDotNetCoreWebAppDemo.Migrations
{
    public partial class ToDoItemUserProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ToDoItem",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItem_UserId",
                table: "ToDoItem",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoItem_AspNetUsers_UserId",
                table: "ToDoItem",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoItem_AspNetUsers_UserId",
                table: "ToDoItem");

            migrationBuilder.DropIndex(
                name: "IX_ToDoItem_UserId",
                table: "ToDoItem");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ToDoItem");
        }
    }
}
