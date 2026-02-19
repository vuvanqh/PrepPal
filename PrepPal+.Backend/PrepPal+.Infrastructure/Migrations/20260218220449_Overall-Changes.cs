using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrepPal_.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OverallChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart Recipe Mappings_Carts_CartId",
                table: "Cart Recipe Mappings");

            migrationBuilder.DropForeignKey(
                name: "FK_Cart Recipe Mappings_Recipes_RecipeId",
                table: "Cart Recipe Mappings");

            migrationBuilder.DropForeignKey(
                name: "FK_CartAccess_Carts_CartId1",
                table: "CartAccess");

            migrationBuilder.DropIndex(
                name: "IX_CartAccess_CartId1",
                table: "CartAccess");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cart Recipe Mappings",
                table: "Cart Recipe Mappings");

            migrationBuilder.DropColumn(
                name: "CartId1",
                table: "CartAccess");

            migrationBuilder.RenameTable(
                name: "Cart Recipe Mappings",
                newName: "CartRecipeMappings");

            migrationBuilder.RenameIndex(
                name: "IX_Cart Recipe Mappings_RecipeId",
                table: "CartRecipeMappings",
                newName: "IX_CartRecipeMappings_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_Cart Recipe Mappings_CartId",
                table: "CartRecipeMappings",
                newName: "IX_CartRecipeMappings_CartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartRecipeMappings",
                table: "CartRecipeMappings",
                columns: new[] { "CartId", "RecipeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CartRecipeMappings_Carts_CartId",
                table: "CartRecipeMappings",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartRecipeMappings_Recipes_RecipeId",
                table: "CartRecipeMappings",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartRecipeMappings_Carts_CartId",
                table: "CartRecipeMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_CartRecipeMappings_Recipes_RecipeId",
                table: "CartRecipeMappings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartRecipeMappings",
                table: "CartRecipeMappings");

            migrationBuilder.RenameTable(
                name: "CartRecipeMappings",
                newName: "Cart Recipe Mappings");

            migrationBuilder.RenameIndex(
                name: "IX_CartRecipeMappings_RecipeId",
                table: "Cart Recipe Mappings",
                newName: "IX_Cart Recipe Mappings_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_CartRecipeMappings_CartId",
                table: "Cart Recipe Mappings",
                newName: "IX_Cart Recipe Mappings_CartId");

            migrationBuilder.AddColumn<Guid>(
                name: "CartId1",
                table: "CartAccess",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cart Recipe Mappings",
                table: "Cart Recipe Mappings",
                columns: new[] { "CartId", "RecipeId" });

            migrationBuilder.CreateIndex(
                name: "IX_CartAccess_CartId1",
                table: "CartAccess",
                column: "CartId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart Recipe Mappings_Carts_CartId",
                table: "Cart Recipe Mappings",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cart Recipe Mappings_Recipes_RecipeId",
                table: "Cart Recipe Mappings",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartAccess_Carts_CartId1",
                table: "CartAccess",
                column: "CartId1",
                principalTable: "Carts",
                principalColumn: "Id");
        }
    }
}
