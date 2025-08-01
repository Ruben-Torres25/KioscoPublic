using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KioscoApp.Migrations
{
    /// <inheritdoc />
    public partial class AddTipoMovimientoCaja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "MovimientosCaja",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "MovimientosCaja");
        }
    }
}
