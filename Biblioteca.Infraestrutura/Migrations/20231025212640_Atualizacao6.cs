using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class Atualizacao6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataStatusItem",
                table: "FichaEmprestimoItens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Quantidade",
                table: "FichaEmprestimoItens",
                type: "decimal(14,2)",
                precision: 14,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "StatusItem",
                table: "FichaEmprestimoItens",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataStatusItem",
                table: "FichaEmprestimoItens");

            migrationBuilder.DropColumn(
                name: "Quantidade",
                table: "FichaEmprestimoItens");

            migrationBuilder.DropColumn(
                name: "StatusItem",
                table: "FichaEmprestimoItens");
        }
    }
}
