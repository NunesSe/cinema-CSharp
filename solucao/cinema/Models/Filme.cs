namespace cinema.Models;
public class Filme
{

    public int Id { get; set; }
    public string? Nome { get; set; }
    public int Duracao { get; set; }
    public Secao? Secao { get; set; }
    public ICollection<Categoria>? Categorias { get; set; }    

}