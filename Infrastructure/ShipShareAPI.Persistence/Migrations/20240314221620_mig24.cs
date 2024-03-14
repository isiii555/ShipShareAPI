using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipShareAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig24 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_RecipientId",
                table: "Reviews",
                column: "RecipientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_SenderId",
                table: "Reviews",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
