import { useState } from "react";
import { Categoria } from "../models/Categoria";
import { useNavigate } from "react-router-dom";

function CadastrarCategoria() {
    const [nome, setNome] = useState("");
    const navigate = useNavigate();
    
    function cadastrarCategoria(e: any) {
        const categoria: Categoria = {
            nome: nome
        }

        fetch("http://localhost:5187/api/categoria/cadastrar", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(categoria),
            })
            e.preventDefault();
        }
        

    return (
        <div>
            <form onSubmit={cadastrarCategoria}>
                <label>Nome: </label>
                <input type="text"onChange={(e: any) => setNome(e.target.value)}/>
                <button type="submit">Cadastrar</button>
            </form>
        </div>
    )
}

export default CadastrarCategoria;