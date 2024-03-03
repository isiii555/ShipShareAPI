using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipShareAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RecipientId",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "Messages");
        }
    }
}
