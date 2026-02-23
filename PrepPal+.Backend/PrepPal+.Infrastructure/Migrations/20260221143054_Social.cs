using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrepPal_.Infrastructure.Migrations;

/// <inheritdoc />
public partial class Social : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Connections",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserId2 = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Connections", x => x.Id);
                table.CheckConstraint("CK_Friends_UserId_Order", "[UserId1]<[UserId2]");
                table.ForeignKey(
                    name: "FK_Connections_Users_UserId1",
                    column: x => x.UserId1,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Connections_Users_UserId2",
                    column: x => x.UserId2,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Messages",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                SenderUsername = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConnectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Messages", x => x.Id);
                table.ForeignKey(
                    name: "FK_Messages_Connections_ConnectionId",
                    column: x => x.ConnectionId,
                    principalTable: "Connections",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Connections_UserId1",
            table: "Connections",
            column: "UserId1");

        migrationBuilder.CreateIndex(
            name: "IX_Connections_UserId1_UserId2",
            table: "Connections",
            columns: new[] { "UserId1", "UserId2" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Connections_UserId2",
            table: "Connections",
            column: "UserId2");

        migrationBuilder.CreateIndex(
            name: "IX_Messages_ConnectionId",
            table: "Messages",
            column: "ConnectionId");

        migrationBuilder.CreateIndex(
            name: "IX_Messages_SenderUsername",
            table: "Messages",
            column: "SenderUsername");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Messages");

        migrationBuilder.DropTable(
            name: "Connections");
    }
}
