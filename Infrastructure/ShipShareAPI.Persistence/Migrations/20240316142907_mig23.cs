using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipShareAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Users_UserId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Post_PostId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_ReviewSenderId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_PostId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Post",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "ItemPhotos",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "ItemType",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "ItemWeight",
                table: "Post");

            migrationBuilder.RenameTable(
                name: "Post",
                newName: "TravellerPosts");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Reviews",
                newName: "ReviewRecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_Post_UserId",
                table: "TravellerPosts",
                newName: "IX_TravellerPosts_UserId");

            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ReviewSenderId",
                table: "Reviews",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TravellerPosts",
                table: "TravellerPosts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SenderPosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemPhotos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemWeight = table.Column<float>(type: "real", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    IsDeclined = table.Column<bool>(type: "bit", nullable: false),
                    Views = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDestination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndDestination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeadlineDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SenderPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SenderPosts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewRecipientId",
                table: "Reviews",
                column: "ReviewRecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_SenderPosts_UserId",
                table: "SenderPosts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_ReviewRecipientId",
                table: "Reviews",
                column: "ReviewRecipientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_ReviewSenderId",
                table: "Reviews",
                column: "ReviewSenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TravellerPosts_Users_UserId",
                table: "TravellerPosts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_ReviewRecipientId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_ReviewSenderId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_TravellerPosts_Users_UserId",
                table: "TravellerPosts");

            migrationBuilder.DropTable(
                name: "SenderPosts");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ReviewRecipientId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TravellerPosts",
                table: "TravellerPosts");

            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "TravellerPosts",
                newName: "Post");

            migrationBuilder.RenameColumn(
                name: "ReviewRecipientId",
                table: "Reviews",
                newName: "SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_TravellerPosts_UserId",
                table: "Post",
                newName: "IX_Post_UserId");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReviewSenderId",
                table: "Reviews",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "PostId",
                table: "Reviews",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Post",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ItemPhotos",
                table: "Post",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemType",
                table: "Post",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "ItemWeight",
                table: "Post",
                type: "real",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Post",
                table: "Post",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_PostId",
                table: "Reviews",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Users_UserId",
                table: "Post",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Post_PostId",
                table: "Reviews",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_ReviewSenderId",
                table: "Reviews",
                column: "ReviewSenderId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
