using System.ComponentModel.DataAnnotations;

public class Genero
{
    [Key]
    public int id_genero { get; set; }
    public string? nombre { get; set; }
}
