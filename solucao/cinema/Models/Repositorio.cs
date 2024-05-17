using System.Collections.Generic;
using System.Linq;

namespace cinema.Models

{
    public class RepositorioFilme
    {
        private List<Filmes> _filmes;
        public RepositorioFilme()

        _filmes = new List<Filmes>
        {
            new filmes{Id = , Nome = "", Duracao = , CategoriaId =},
            new filmes{Id = , Nome = "", Duracao = , CategoriaId =},
            new filmes{Id = , Nome = "", Duracao = , CategoriaId =},
            new filmes{Id = , Nome = "", Duracao = , CategoriaId =}  
        };
    }

    public List<Filme> ObterFilmePelaCategoria(int categoriaId)
    {
        return _filmes.Where(x => x.CategoriaId == categoriaId). ToList();
    }
}
