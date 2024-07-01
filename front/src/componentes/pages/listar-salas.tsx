// componentes/pages/ListarSalas.tsx

import React, { useState, useEffect } from "react";
import { Sala } from "../models/Sala";
import { Link } from "react-router-dom";

function ListarSalas() {
  const [salas, setSalas] = useState<Sala[]>([]);

  useEffect(() => {
    carregarSalas();
  }, []);

  function carregarSalas() {
    fetch("http://localhost:5187/api/sala/listar")
      .then((resposta) => resposta.json())
      .then((data) => {
        if (Array.isArray(data)) {
          setSalas(data);
        } else {
          setSalas([]);
        }
      })
      .catch(() => {
        setSalas([]);
      });
  }

  function deletarSala(id: number) {
    fetch(`http://localhost:5187/api/sala/excluir/${id}`, {
      method: "DELETE",
    })
      .then(() => {
        carregarSalas();
      })
      
  }

  return (
    <div>
      <h1>Listar Salas</h1>
      <table>
        <thead>
          <tr>
            <th>#</th>
            <th>Quantidade de Assentos</th>
            <th>Assentos Ocupados</th>
            <th>Alterar</th>
            <th>Deletar</th>
          </tr>
        </thead>
        <tbody>
          {salas.map((sala) => (
            <tr key={sala.id}>
              <td>{sala.id}</td>
              <td>{sala.quantidadeAssentos}</td>
              <td>{sala.assentosOcupados}</td>
              <td>
                <Link to={`/salas/alterar/${sala.id}`}>Alterar</Link>
              </td>
              <td>
                <button onClick={() => deletarSala(sala.id!)}>Deletar</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default ListarSalas;
