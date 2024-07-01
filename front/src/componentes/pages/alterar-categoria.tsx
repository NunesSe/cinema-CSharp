import { useState } from "react";
import { Categoria } from "../models/Categoria";
import { useNavigate, useParams } from "react-router-dom";

function AlterarCategoria() {
  const navigate = useNavigate();
  const { id } = useParams();
  const [nome, setNome] = useState("");

  function alterarCategoria(e: any) {
    e.preventDefault();
    const categoriaAtualizada: Categoria = {
      nome: nome
    };

    fetch(`http://localhost:5187/api/categoria/alterar/${id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(categoriaAtualizada),
    })
      .then((resposta) => resposta.json())
      .then(() => {
        navigate("/categoria/listar");
      });
  }

  return (
    <div>
      <h1>Alterar Categoria</h1>
      <form onSubmit={alterarCategoria}>
        <label>Nome: </label>
        <input
          type="text"
          value={nome}
          onChange={(e: any) => setNome(e.target.value)}
          required
        />
        <br />
        <button type="submit">Salvar</button>
      </form>
    </div>
  );
}

export default AlterarCategoria;
