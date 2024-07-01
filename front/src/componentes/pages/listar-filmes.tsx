import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { Filme } from "../models/Filme";

function ListarFilmes() {
  const [filmes, setFilmes] = useState<Filme[]>([]);

  useEffect(() => {
    carregarFilmes();
  }, []);

  function carregarFilmes() {
    fetch("http://localhost:5187/api/filme/listar")
      .then((resposta) => resposta.json())
      .then((data) => {
        if (Array.isArray(data)) {
          setFilmes(data);
          console.log(data);
        } else {
          setFilmes([]);
        }
      })
      .catch(() => {
        setFilmes([]);
      });
  }

  function deletarFilme(id: string) {
    fetch(`http://localhost:5187/api/filme/deletar/${id}`, {
      method: "DELETE",
    })
      .then((resposta) => resposta.json())
      .then(() => {
        carregarFilmes();
      })
      .catch((error) => {
        console.error("Erro ao deletar filme:", error);
      });
  }

  return (
    <div>
      <h1>Listar Filmes</h1>
      <table border={1}>
        <thead>
          <tr>
            <th>#</th>
            <th>Nome</th>
            <th>Duração</th>
            <th>CategoriaId</th>
            <th>Alterar</th>
            <th>Deletar</th>
          </tr>
        </thead>
        <tbody>
          {filmes.map((filme) => (
            <tr key={filme.id}>
              <td>{filme.id}</td>
              <td>{filme.nome}</td>
              <td>{filme.duracao} min</td>
              <td>{filme.categoriaId}</td>
              <td>
                <Link to={`/filmes/alterar/${filme.id}`}>Alterar</Link>
              </td>
              <td>
                <button onClick={() => deletarFilme(filme.id!)}>Deletar</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default ListarFilmes;
