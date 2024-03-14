
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
            migrationBuilder.CreateTable(
            name: "Reviews",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                ReviewSenderId = table.Column<Guid>(nullable: false),
                ReviewRecipientId = table.Column<Guid>(nullable: false),
                Rating = table.Column<int>(nullable: false),
                Text = table.Column<string>(nullable: true),
                IsConfirmed = table.Column<bool>(nullable: false),
                IsDeclined = table.Column<bool>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Reviews", x => x.Id);
            });
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
