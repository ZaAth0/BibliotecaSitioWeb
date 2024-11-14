using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bibliotecaaa.Migrations
{
    /// <inheritdoc />
    public partial class AddAutorIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_libros_Autores_autor_id",
                table: "libros");

            migrationBuilder.RenameColumn(
                name: "autor_id",
                table: "libros",
                newName: "IdAutor");

            migrationBuilder.RenameIndex(
                name: "IX_libros_autor_id",
                table: "libros",
                newName: "IX_libros_IdAutor");

            migrationBuilder.AddForeignKey(
                name: "FK_libros_Autores_IdAutor",
                table: "libros",
                column: "IdAutor",
                principalTable: "Autores",
                principalColumn: "IdAutor",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_libros_Autores_IdAutor",
                table: "libros");

            migrationBuilder.RenameColumn(
                name: "IdAutor",
                table: "libros",
                newName: "autor_id");

            migrationBuilder.RenameIndex(
                name: "IX_libros_IdAutor",
                table: "libros",
                newName: "IX_libros_autor_id");

            migrationBuilder.AddForeignKey(
                name: "FK_libros_Autores_autor_id",
                table: "libros",
                column: "autor_id",
                principalTable: "Autores",
                principalColumn: "IdAutor",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
