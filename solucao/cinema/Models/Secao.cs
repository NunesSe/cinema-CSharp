namespace cinema.Models;

public class Secao
{
    public int Id { get; set; }
    public string? HorarioInicio { get; set; }
    public string? HorarioFinal{ get; set; }

    public int Dia { get; set; }
    public int Mes { get; set; }

    public int FilmeId { get; set; }
    public Filme? Filme { get; set; }

    public int SalaId { get; set; }
    public Sala? Sala { get; set; }
}