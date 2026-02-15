using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrepPal_.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedRefreshTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpirationDateTime",
                table: "Users",
                newName: "TokenIssuedAt");

            migrationBuilder.RenameColumn(
                name: "RefreshToken",
                table: "Users",
                newName: "TokenHash");

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenExpirationDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TokenExpirationDate",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "TokenIssuedAt",
                table: "Users",
                newName: "RefreshTokenExpirationDateTime");

            migrationBuilder.RenameColumn(
                name: "TokenHash",
                table: "Users",
                newName: "RefreshToken");
        }
    }
}
