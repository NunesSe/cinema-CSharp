namespace cinema.Models;
public class ReservaSessao
{
    public int ReservaSessaoId { get; set; }
    public int ReservaId { get; set; }
    public Reserva Reserva { get; set; }

    public int SessaoId { get; set; }
    public Sessao Sessao { get; set; }
}