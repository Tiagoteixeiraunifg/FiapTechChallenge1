using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class Migration4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LivroAutores_Autores_AutorId",
                table: "LivroAutores");

            migrationBuilder.AlterColumn<int>(
                name: "AutorId",
                table: "LivroAutores",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LivroAutores_Autores_AutorId",
                table: "LivroAutores",
                column: "AutorId",
                principalTable: "Autores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LivroAutores_Autores_AutorId",
                table: "LivroAutores");

            migrationBuilder.AlterColumn<int>(
                name: "AutorId",
                table: "LivroAutores",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_LivroAutores_Autores_AutorId",
                table: "LivroAutores",
                column: "AutorId",
                principalTable: "Autores",
                principalColumn: "Id");
        }
    }
}
