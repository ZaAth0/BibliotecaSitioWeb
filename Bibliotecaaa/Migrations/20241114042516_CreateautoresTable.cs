using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bibliotecaaa.Migrations
{
    /// <inheritdoc />
    public partial class CreateautoresTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "genero",
                table: "libros",
                type: "int",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Generos",
                columns: table => new
                {
                    id_genero = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Generos", x => x.id_genero);
                });

            migrationBuilder.CreateIndex(
                name: "IX_libros_genero",
                table: "libros",
                column: "genero");

            migrationBuilder.AddForeignKey(
                name: "FK_libros_Generos_genero",
                table: "libros",
                column: "genero",
                principalTable: "Generos",
                principalColumn: "id_genero",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_libros_Generos_genero",
                table: "libros");

            migrationBuilder.DropTable(
                name: "Generos");

            migrationBuilder.DropIndex(
                name: "IX_libros_genero",
                table: "libros");

            migrationBuilder.AlterColumn<string>(
                name: "genero",
                table: "libros",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldUnicode: false,
                oldMaxLength: 50);
        }
    }
}
