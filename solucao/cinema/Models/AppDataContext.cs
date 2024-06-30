namespace cinema.Models;

using Microsoft.EntityFrameworkCore;

public class AppDataContext : DbContext
{
    public DbSet<Filme> Filmes{ get; set; }
    public DbSet<Categoria> Categorias{ get; set; }
    public DbSet<Sala> Salas { get; set; }
    public DbSet<Sessao> Sessoes { get; set;}
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=banco.db");

        modelBuilder.Entity<ReservaSessao>()
            .HasKey(rs => new { rs.ReservaId, rs.SessaoId });

        modelBuilder.Entity<ReservaSessao>()
            .HasOne(rs => rs.Reserva)
            .WithMany(r => r.ReservaSessoes)
            .HasForeignKey(rs => rs.ReservaId);

        modelBuilder.Entity<ReservaSessao>()
            .HasOne(rs => rs.Sessao)
            .WithMany(s => s.ReservaSessoes)
            .HasForeignKey(rs => rs.SessaoId);
    }
}