import React, { useEffect, useState } from "react";
import { Filme } from "../models/Filme";
import { useNavigate } from "react-router-dom";
import { Categoria } from "../models/Categoria";

function CadastrarFilme() {
  const [nome, setNome] = useState("");
  const [duracao, setDuracao] = useState("");
  const [categoriaId, setCategoriaId] = useState("");
  const [categorias, setCategorias] = useState<Categoria[]>([]);
  const navigate = useNavigate();

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

  function cadastrarFilme(e: React.FormEvent) {
    e.preventDefault();

    const filme: Filme = {
      nome: nome,
      duracao: parseInt(duracao),
      CategoriaId: parseInt(categoriaId),
    };

    fetch("http://localhost:5187/api/filme/cadastrar", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(filme),
        })
        e.preventDefault();
  }

  return (
    <div>
      <h1>Cadastrar Filme</h1>
      <form onSubmit={cadastrarFilme}>
        <label>Nome:</label>
        <input type="text" onChange={(e) => setNome(e.target.value)} required />
        <br />
        <label>Duração (minutos):</label>
        <input type="number" onChange={(e) => setDuracao(e.target.value)} required />
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
        <button type="submit">Cadastrar</button>
      </form>
    </div>
  );
}

export default CadastrarFilme;
