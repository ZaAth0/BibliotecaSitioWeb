using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bibliotecaaa.Migrations
{
    /// <inheritdoc />
    public partial class UpdateautoresTabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "autor",
                table: "libros");

            migrationBuilder.AddColumn<int>(
                name: "autor_id",
                table: "libros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Autores",
                columns: table => new
                {
                    IdAutor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AutorNombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autores", x => x.IdAutor);
                });

            migrationBuilder.CreateIndex(
                name: "IX_libros_autor_id",
                table: "libros",
                column: "autor_id");

            migrationBuilder.AddForeignKey(
                name: "FK_libros_Autores_autor_id",
                table: "libros",
                column: "autor_id",
                principalTable: "Autores",
                principalColumn: "IdAutor",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_libros_Autores_autor_id",
                table: "libros");

            migrationBuilder.DropTable(
                name: "Autores");

            migrationBuilder.DropIndex(
                name: "IX_libros_autor_id",
                table: "libros");

            migrationBuilder.DropColumn(
                name: "autor_id",
                table: "libros");

            migrationBuilder.AddColumn<string>(
                name: "autor",
                table: "libros",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
