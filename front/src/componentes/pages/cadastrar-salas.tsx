// componentes/pages/CadastrarSala.tsx

import React, { useState } from "react";
import { Sala } from "../models/Sala";
import { useNavigate } from "react-router-dom";

function CadastrarSala() {
  const [quantidadeAssentos, setQuantidadeAssentos] = useState("");
  const [assentosOcupados, setAssentosOcupados] = useState("");
  const navigate = useNavigate();

  function cadastrarSala(e: React.FormEvent) {
    e.preventDefault();

    const sala: Sala = {
      quantidadeAssentos: parseInt(quantidadeAssentos),
      assentosOcupados: parseInt(assentosOcupados),
    };

    fetch("http://localhost:5187/api/sala/cadastrar", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(sala),
    })
      .then(() => {
        navigate("/salas/listar");
      })
      .catch((error) => {
        console.error("Erro ao cadastrar sala:", error);
      });
  }

  return (
    <div>
      <h1>Cadastrar Sala</h1>
      <form onSubmit={cadastrarSala}>
        <label>Quantidade de Assentos:</label>
        <input type="number" onChange={(e) => setQuantidadeAssentos(e.target.value)} required />
        <br />
        <label>Assentos Ocupados:</label>
        <input type="number" onChange={(e) => setAssentosOcupados(e.target.value)} required />
        <br />
        <button type="submit">Cadastrar</button>
      </form>
    </div>
  );
}

export default CadastrarSala;
