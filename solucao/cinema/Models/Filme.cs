namespace cinema.Models;
public class Filme
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public int Duracao { get; set; }
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
        
    }