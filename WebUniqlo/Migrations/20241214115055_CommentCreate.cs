using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebUniqlo.Migrations
{
    /// <inheritdoc />
    public partial class CommentCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRatings_AspNetUsers_UserId1",
                table: "ProductRatings");

            migrationBuilder.DropIndex(
                name: "IX_ProductRatings_UserId1",
                table: "ProductRatings");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "ProductRatings");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ProductRatings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductRatings_UserId",
                table: "ProductRatings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRatings_AspNetUsers_UserId",
                table: "ProductRatings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRatings_AspNetUsers_UserId",
                table: "ProductRatings");

            migrationBuilder.DropIndex(
                name: "IX_ProductRatings_UserId",
                table: "ProductRatings");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ProductRatings",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "ProductRatings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductRatings_UserId1",
                table: "ProductRatings",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRatings_AspNetUsers_UserId1",
                table: "ProductRatings",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
