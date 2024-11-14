using Bibliotecaaa.Models;
using System.ComponentModel.DataAnnotations;

namespace Bibliotecaaa.Models
{

    public partial class Libro
    {
        public int IdLibro { get; set; }  // Clave primaria del libro

        public string Titulo { get; set; } = null!;  // Título del libro

        public int Genero { get; set; }  // Clave foránea a Genero
        public Genero? GeneroNavigation { get; set; }  // Propiedad de navegación con Genero

        public int AutorRel { get; set; }  // Clave foránea a Autor (ID del autor)
        public Autor? AutorNavigation { get; set; }  // Propiedad de navegación con Autor

        public string? Descripcion { get; set; }  // Descripción del libro
        public string? Portada { get; set; }  // URL de la portada

        [Display(Name = "Fecha de publicación")]
        public DateOnly? FechaPublicacion { get; set; }  // Fecha de publicación (opcional)

        public bool Disponible { get; set; }  // Indica si el libro está disponible (opcional)

        public DateTime? FechaAgregado { get; set; }  // Fecha en la que se agregó el libro al sistema

        // Propiedad "caché" para el nombre del autor
        public string? AutorNombre => AutorNavigation?.AutorNombre;
    }
}