using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Franquias.Api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaStatusAtivoNaUnidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ativa",
                table: "Unidades",
                newName: "Ativo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ativo",
                table: "Unidades",
                newName: "Ativa");
        }
    }
}
