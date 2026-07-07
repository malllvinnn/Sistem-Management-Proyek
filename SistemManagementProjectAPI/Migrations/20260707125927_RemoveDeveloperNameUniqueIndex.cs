using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemManagementProjectAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDeveloperNameUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Developers_Name",
                table: "Developers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Developers_Name",
                table: "Developers",
                column: "Name",
                unique: true);
        }
    }
}
