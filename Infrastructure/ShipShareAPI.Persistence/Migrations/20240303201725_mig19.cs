using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipShareAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConversationUser_Conversations_ConversationsId",
                table: "ConversationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ConversationUser_Users_UsersId",
                table: "ConversationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConversationUser",
                table: "ConversationUser");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "ConversationUser",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ConversationsId",
                table: "ConversationUser",
                newName: "ConversationId");

            migrationBuilder.RenameIndex(
                name: "IX_ConversationUser_UsersId",
                table: "ConversationUser",
                newName: "IX_ConversationUser_UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ConversationUser",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ConversationUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "ConversationUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConversationUser",
                table: "ConversationUser",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationUser_ConversationId",
                table: "ConversationUser",
                column: "ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationUser_Conversations_ConversationId",
                table: "ConversationUser",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationUser_Users_UserId",
                table: "ConversationUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConversationUser_Conversations_ConversationId",
                table: "ConversationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ConversationUser_Users_UserId",
                table: "ConversationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConversationUser",
                table: "ConversationUser");

            migrationBuilder.DropIndex(
                name: "IX_ConversationUser_ConversationId",
                table: "ConversationUser");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ConversationUser");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ConversationUser");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "ConversationUser");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ConversationUser",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "ConversationId",
                table: "ConversationUser",
                newName: "ConversationsId");

            migrationBuilder.RenameIndex(
                name: "IX_ConversationUser_UserId",
                table: "ConversationUser",
                newName: "IX_ConversationUser_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConversationUser",
                table: "ConversationUser",
                columns: new[] { "ConversationsId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationUser_Conversations_ConversationsId",
                table: "ConversationUser",
                column: "ConversationsId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationUser_Users_UsersId",
                table: "ConversationUser",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
