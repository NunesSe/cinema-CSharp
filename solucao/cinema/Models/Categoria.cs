namespace cinema.Models;

public class Categoria
{
    public int Id { get; set; }
    public string? Nome { get; set; }

    public int FilmeId { get; set; }
    public List<Filme>? Filme { get; }
}