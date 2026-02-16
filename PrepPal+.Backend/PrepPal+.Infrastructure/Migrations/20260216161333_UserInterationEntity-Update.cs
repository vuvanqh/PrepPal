using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrepPal_.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserInterationEntityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExternalRecipeId",
                table: "UserRecipeInteractions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserRecipeInteractions_ExternalRecipeId",
                table: "UserRecipeInteractions",
                column: "ExternalRecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserRecipeInteractions_ExternalRecipeId",
                table: "UserRecipeInteractions");

            migrationBuilder.DropColumn(
                name: "ExternalRecipeId",
                table: "UserRecipeInteractions");
        }
    }
}
