import React from "react";
import ListarCategoria from "./componentes/pages/listar-categoria";
import CadastrarCategoria from "./componentes/pages/cadastrar-categoria";
import { BrowserRouter, Link, Route, Routes } from "react-router-dom";
import CategoriaAlterar from "./componentes/pages/alterar-categoria";
import ListarFilmes from "./componentes/pages/listar-filmes";
import CadastrarFilme from "./componentes/pages/cadastrar-filme";
import AlterarFilme from "./componentes/pages/alterar-filme";
import ListarSalas from "./componentes/pages/listar-salas";
import CadastrarSala from "./componentes/pages/cadastrar-salas";
import AlterarSala from "./componentes/pages/alterar-sala";

function App() {
  return (
    <>
      <BrowserRouter>
        <div className="App">
          <nav>
            <ul>
              <li>
                <Link to="/categoria/listar">Lista de Categorias</Link>
              </li>
              <li>
                <Link to="/categoria/cadastro">Cadastro de Categoria</Link>
              </li>
              <li>
                <Link to="/filmes/listar">Lista de Filme</Link>
              </li>
              <li>
                <Link to="/filmes/cadastro">Cadastro de Filme</Link>
              </li>
              <li>
                <Link to="/salas/listar">Lista de Salas</Link>
              </li>
              <li>
                <Link to="/salas/cadastro">Cadastro de Sala</Link>
              </li>
            </ul>
          </nav>
          <Routes>
            <Route path="/" element={<ListarCategoria />} />
            <Route path="/categoria/listar" element={<ListarCategoria />} />
            <Route
              path="/categoria/cadastro"
              element={<CadastrarCategoria />}
            />
            <Route
              path="/categoria/alterar/:id"
              element={<CategoriaAlterar />}
            />
            <Route path="/filmes/listar" element={<ListarFilmes />} />
            <Route path="/filmes/cadastro" element={<CadastrarFilme />} />
            <Route path="/filmes/alterar/:id" element={<AlterarFilme />} />
            <Route path="/salas/listar" element={<ListarSalas />} />
            <Route path="/salas/cadastro" element={<CadastrarSala />} />
            <Route path="/salas/alterar/:id" element={<AlterarSala />} />
          </Routes>
        </div>
      </BrowserRouter>
    </>
  );
}

export default App;
