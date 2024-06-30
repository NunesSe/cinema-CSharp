namespace cinema.Models;
public class Reserva
{
    public int Id { get; set; }
    public string ClienteNome { get; set; }
    public List<ReservaSessao> ReservaSessoes { get; set; }
}

