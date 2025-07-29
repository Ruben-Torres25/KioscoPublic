using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KioscoApp.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cuit",
                table: "Proveedores",
                newName: "CUIT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CUIT",
                table: "Proveedores",
                newName: "Cuit");
        }
    }
}
