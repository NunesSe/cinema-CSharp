using System.Collections.Generic;

namespace cinema.Models
{
    public class ServicoCinema
    {
        private readonly RepositorioFilme _repositorioFilme;

        public ServicoCinema(RepositorioFilme repositorioFilme)
        {
            _repositorioFilme = repositorioFilme;
            
        }
        public List<Filme> MostrarFilmesPelaCategoria(int categoriaId)
        {
            return _repositorioFilme.ObterFilmePelaCategoria(categoriaId);
        }
    }
}