using cinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();
var app = builder.Build();

// Cadastrar uma categoria
// http://localhost:5187/api/categoria/cadastrar
app.MapPost("/api/categoria/cadastrar", ([FromBody] Categoria categoria, [FromServices] AppDataContext ctx) =>{
    // TODO: VALIDAR SE CATEGORIA COM O MESMO NOME JA EXISTE
    ctx.Categorias.Add(categoria);
    ctx.SaveChanges();
    return Results.Created("Categoria criada!", categoria);

}); 

// Listar uma categoria
// http://localhost:5187/api/categoria/listar
app.MapGet("/api/categoria/listar",([FromServices] AppDataContext ctx ) =>{
    // TODO: MOSTRAR MENSAGEM DIFERENTE CASO NÃO TENHA CATEGORIAS CADASTRADAS
    var categorias = ctx.Categorias.ToList();
    return Results.Ok(categorias);
});

// Alterar uma categoria
// TODO: MUDAR O NOME DA CATEGORIA, SEMPRE VALIDEM SE ALGO É VALIDO, EX: CATEGORIA EXISTE? ETC

// DELETAR UMA CATEGORIA
// TODO: FAZER OPERAÇÃO, SEMPRE VALIDEM SE ALGO É VALIDO, EX: CATEGORIA EXISTE? ETC

// Adicionar um filme
// http://localhost:5187/api/filme/cadastrar
app.MapPost("/api/filme/cadastrar", (Filme filme, [FromServices] AppDataContext ctx) =>
{
    // TODO: MOSTRAR MENSAGEM CASO TENTE CADASTRAR UM FILME COM O MESMO NOME
    Categoria? categoria = ctx.Categorias.FirstOrDefault(x => x.Id == filme.CategoriaId);
    if (categoria is null)
    {
        return Results.NotFound("Categoria não encontrada!");
    }

    filme.Categoria = categoria;
    ctx.Filmes.Add(filme);
    ctx.SaveChanges();

    return Results.Created("", filme);
});

// Listar filmes
// http://localhost:5187/api/filme/listar
app.MapGet("/api/filme/listar", ([FromServices] AppDataContext ctx) =>{
    // MOSTRAR MENSAGEM CASO NAO HAJA FILMES CADASTRADOS
    return Results.Ok(ctx.Filmes.Include(f => f.Categoria).ToList());
});

// Alterar um filme
// TODO: MUDAR O NOME DO FILME

// DELETAR UM FILME
// TODO: FAZER OPERAÇÃO, 

app.Run();
