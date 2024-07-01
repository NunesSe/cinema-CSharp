import { useEffect, useState } from "react";
import { Categoria } from "../models/Categoria";
import { Link } from "react-router-dom";

function ListarCategoria() {
  const [categorias, setCategoria] = useState<Categoria[]>([]);

  useEffect(() => {
    carregarCategorias();
  }, []);

  function carregarCategorias() {
    fetch("http://localhost:5187/api/categoria/listar")
      .then((resposta) => resposta.json())
      .then((data) => {
        if (Array.isArray(data)) {
          setCategoria(data);
        } else {
          setCategoria([]); 
        }
      })
      .catch(() => {
        setCategoria([]); 
      });
  }

  function deletar(id: string) {
    fetch("http://localhost:5187/api/categoria/deletar/" + id, {
      method: "DELETE",
    })
      .then((resposta) => resposta.json())
      .then((data) => {
        if (Array.isArray(data)) {
          setCategoria(data);
        } else {
          setCategoria([]); 
        }
      })
      .catch(() => {
        setCategoria([]); 
      });
  }

  return (
    <div>
      <h1>Listar Categorias</h1>
      <table border={1}>
        <thead>
          <tr>
            <th>#</th>
            <th>Nome</th>
            <th>Deletar</th>
            <th>Alterar</th>
          </tr>
        </thead>
        <tbody>
          {categorias.length > 0 ? (
            categorias.map((categoria) => (
              <tr key={categoria.id}>
                <td>{categoria.id}</td>
                <td>{categoria.nome}</td>
                <td>
                  <button
                    onClick={() => {
                      deletar(categoria.id!);
                    }}
                  >
                    Deletar
                  </button>
                </td>
                <td>
                  <Link to={`/categoria/alterar/${categoria.id}`}>
                    Alterar
                  </Link>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan={4}>Nenhuma categoria encontrada</td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}

export default ListarCategoria;
