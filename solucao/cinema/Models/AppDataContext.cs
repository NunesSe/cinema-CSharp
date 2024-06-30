namespace cinema.Models;

using Microsoft.EntityFrameworkCore;

public class AppDataContext : DbContext
{
    public DbSet<Filme> Filmes{ get; set; }
    public DbSet<Categoria> Categorias{ get; set; }
    public DbSet<Sala> Salas { get; set; }
    public DbSet<Sessao> Sessoes { get; set;}
    public DbSet<ReservaSessao> ReservarSessoes { get; set;}

    public DbSet<Reserva> Reservas { get; set;}
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=banco.db");
    }
}