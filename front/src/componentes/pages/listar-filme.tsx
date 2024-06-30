import { useEffect, useState } from "react";
import { Filme } from "../../../models/Filme";
import { Link } from "react-router-dom";

function FilmesListar() {
  const [filmes, setFilmes] = useState<Filme[]>([]);

  useEffect(() => {
    carregarProdutos();
  }, []);

  function carregarProdutos() {
    //FETCH ou AXIOS
    fetch("http://localhost:5225/api/produto/listar")
      .then((resposta) => resposta.json())
      .then((filmes: Filme[]) => {
        console.table(Filmes);
        setFilmes(Filmes);
      });
  }

  function deletar(id: string) {
    console.log(`Id: ${id}`);
    axios
      .delete(`http://localhost:5225/api/produto/deletar/${id}`)
      .then((resposta) => {
        console.log(resposta.data);
        setProdutos(resposta.data);
      });
  }

  return (
    <div>
      <h1>Listar Produtos</h1>
      <table border={1}>
        <thead>
          <tr>
            <th>#</th>
            <th>Nome</th>
            <th>Descrição</th>
            <th>Quantidade</th>
            <th>Valor</th>
            <th>Criado Em</th>
            <th>Deletar</th>
            <th>Alterar</th>
          </tr>
        </thead>
        <tbody>
          {produtos.map((produto) => (
            <tr key={produto.id}>
              <td>{produto.id}</td>
              <td>{produto.nome}</td>
              <td>{produto.descricao}</td>
              <td>{produto.quantidade}</td>
              <td>{produto.valor}</td>
              <td>{produto.criadoEm}</td>
              <td>
                <button
                  onClick={() => {
                    deletar(produto.id!);
                  }}
                >
                  Deletar
                </button>
              </td>
              <td>
                <Link to={`/pages/produto/alterar/${produto.id}`}>
                  Alterar
                </Link>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default ProdutoListar;