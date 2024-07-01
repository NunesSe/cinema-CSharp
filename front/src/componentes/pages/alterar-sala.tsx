// componentes/pages/AlterarSala.tsx

import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Sala } from "../models/Sala";

function AlterarSala() {
  const navigate = useNavigate();
  const { id } = useParams();
  const [quantidadeAssentos, setQuantidadeAssentos] = useState("");
  const [assentosOcupados, setAssentosOcupados] = useState("");

  useEffect(() => {
    if (id) {
      fetch(`http://localhost:5187/api/sala/buscar/${id}`)
        .then((resposta) => resposta.json())
        .then((sala: Sala) => {
          setQuantidadeAssentos(sala.quantidadeAssentos.toString());
          setAssentosOcupados(sala.assentosOcupados.toString());
        })
        .catch((error) => {
          console.error("Erro ao buscar sala:", error);
        });
    }
  }, [id]);

  function alterarSala(e: React.FormEvent) {
    e.preventDefault();

    const salaAtualizada: Sala = {
      quantidadeAssentos: parseInt(quantidadeAssentos),
      assentosOcupados: parseInt(assentosOcupados),
    };

    fetch(`http://localhost:5187/api/sala/alterar/${id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(salaAtualizada),
    })
      .then(() => {
        navigate("/salas/listar");
      })
      .catch((error) => {
        console.error("Erro ao alterar sala:", error);
      });
  }

  return (
    <div>
      <h1>Alterar Sala</h1>
      <form onSubmit={alterarSala}>
        <label>Quantidade de Assentos:</label>
        <input type="number" value={quantidadeAssentos} onChange={(e) => setQuantidadeAssentos(e.target.value)} required />
        <br />
        <label>Assentos Ocupados:</label>
        <input type="number" value={assentosOcupados} onChange={(e) => setAssentosOcupados(e.target.value)} required />
        <br />
        <button type="submit">Salvar</button>
      </form>
    </div>
  );
}

export default AlterarSala;
