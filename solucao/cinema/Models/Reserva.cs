public class Reserva
{
    public int Id { get; set; }
    public string ClienteNome { get; set; }
    public List<ReservaSessao> ReservaSessoes { get; set; }
}

public class ReservaSessao
{
    public int ReservaId { get; set; }
    public Reserva Reserva { get; set; }

    public int SessaoId { get; set; }
    public Sessao Sessao { get; set; }
}