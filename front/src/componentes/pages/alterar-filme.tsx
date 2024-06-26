import React, { useState, useEffect } from "react";
import { Filme } from "../models/Filme";
import { useParams, useNavigate } from "react-router-dom";
import { Categoria } from "../models/Categoria";

function AlterarFilme() {
  const navigate = useNavigate();
  const { id } = useParams();
  const [nome, setNome] = useState("");
  const [duracao, setDuracao] = useState("");
  const [categoriaId, setCategoriaId] = useState("");
  const [categorias, setCategorias] = useState<Categoria[]>([]);

  useEffect(() => {
    carregarCategorias();
  }, []);

  function carregarCategorias() {
    fetch("http://localhost:5187/api/categoria/listar")
      .then((resposta) => resposta.json())
      .then((data) => {
        if (Array.isArray(data)) {
          setCategorias(data);
        } else {
          setCategorias([]);
        }
      })
      .catch(() => {
        setCategorias([]);
      });
  }

  function alterarFilme(e: React.FormEvent) {
    e.preventDefault();

    const filmeAtualizado: Filme = {
      nome: nome,
      duracao: parseInt(duracao),
      CategoriaId: parseInt(categoriaId),
    };

    fetch(`http://localhost:5187/api/filme/alterar/${id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(filmeAtualizado),
    })
      .then((resposta) => resposta.json())
      .then(() => {
        navigate("/filmes/listar");
      })
  }

  return (
    <div>
      <h1>Alterar Filme</h1>
      <form onSubmit={alterarFilme}>
        <label>Nome:</label>
        <input type="text" value={nome} onChange={(e) => setNome(e.target.value)} required />
        <br />
        <label>Duração (minutos):</label>
        <input type="number" value={duracao} onChange={(e) => setDuracao(e.target.value)} required />
        <br />
        <label>Categoria:</label>
        <select value={categoriaId} onChange={(e) => setCategoriaId(e.target.value)} required>
          {categorias.map((categoria) => (
            <option key={categoria.id} value={categoria.id}>
              {categoria.nome}
            </option>
          ))}
        </select>
        <br />
        <button type="submit">Salvar</button>
      </form>
    </div>
  );
}

export default AlterarFilme;
