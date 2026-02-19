using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrepPal_.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Cart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Ingredients");

            migrationBuilder.AddColumn<string>(
                name: "AliasName",
                table: "Ingredients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cart Recipe Mappings",
                columns: table => new
                {
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecipeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart Recipe Mappings", x => new { x.CartId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_Cart Recipe Mappings_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cart Recipe Mappings_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartAccess",
                columns: table => new
                {
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccessType = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CartId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartAccess", x => new { x.CartId, x.UserId });
                    table.ForeignKey(
                        name: "FK_CartAccess_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartAccess_Carts_CartId1",
                        column: x => x.CartId1,
                        principalTable: "Carts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CartAccess_Users_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CartAccess_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cart Recipe Mappings_CartId",
                table: "Cart Recipe Mappings",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart Recipe Mappings_RecipeId",
                table: "Cart Recipe Mappings",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_CartAccess_ApplicationUserId",
                table: "CartAccess",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CartAccess_CartId",
                table: "CartAccess",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartAccess_CartId1",
                table: "CartAccess",
                column: "CartId1");

            migrationBuilder.CreateIndex(
                name: "IX_CartAccess_UserId",
                table: "CartAccess",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_OwnerId",
                table: "Carts",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cart Recipe Mappings");

            migrationBuilder.DropTable(
                name: "CartAccess");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropColumn(
                name: "AliasName",
                table: "Ingredients");

            migrationBuilder.AddColumn<int>(
                name: "ExternalId",
                table: "Ingredients",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
