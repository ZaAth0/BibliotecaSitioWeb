using System.ComponentModel.DataAnnotations;

namespace Bibliotecaaa.Models
{
    public class Autor
    {
        [Key]
        public int IdAutor { get; set; }
        public string AutorNombre { get; set; }
    }

}
