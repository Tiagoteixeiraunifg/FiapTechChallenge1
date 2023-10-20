using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class TesteDeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LivroAutores_Livros_LivroId",
                table: "LivroAutores");

            migrationBuilder.DropForeignKey(
                name: "FK_Livros_Editoras_EditoraId",
                table: "Livros");

            migrationBuilder.AddForeignKey(
                name: "FK_LivroAutores_Livros_LivroId",
                table: "LivroAutores",
                column: "LivroId",
                principalTable: "Livros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Livros_Editoras_EditoraId",
                table: "Livros",
                column: "EditoraId",
                principalTable: "Editoras",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LivroAutores_Livros_LivroId",
                table: "LivroAutores");

            migrationBuilder.DropForeignKey(
                name: "FK_Livros_Editoras_EditoraId",
                table: "Livros");

            migrationBuilder.AddForeignKey(
                name: "FK_LivroAutores_Livros_LivroId",
                table: "LivroAutores",
                column: "LivroId",
                principalTable: "Livros",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Livros_Editoras_EditoraId",
                table: "Livros",
                column: "EditoraId",
                principalTable: "Editoras",
                principalColumn: "Id");
        }
    }
}
