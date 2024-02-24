using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShipShareAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ItemPhoto",
                table: "Post",
                newName: "ItemPhotos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ItemPhotos",
                table: "Post",
                newName: "ItemPhoto");
        }
    }
}
