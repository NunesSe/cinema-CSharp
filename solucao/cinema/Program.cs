using cinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();
var app = builder.Build();

// Cadastrar uma categoria
// http://localhost:5187/api/categoria/cadastrar
app.MapPost("/api/categoria/cadastrar", ([FromBody] Categoria categoria, [FromServices] AppDataContext ctx) =>
{
    // TODO: VALIDAR SE CATEGORIA COM O MESMO NOME JA EXISTE
    ctx.Categorias.Add(categoria);
    ctx.SaveChanges();
    return Results.Created("Categoria criada!", categoria);

});

// Listar uma categoria
// http://localhost:5187/api/categoria/listar
app.MapGet("/api/categoria/listar", ([FromServices] AppDataContext ctx) =>
{
    // TODO: MOSTRAR MENSAGEM DIFERENTE CASO NÃO TENHA CATEGORIAS CADASTRADAS
    var categorias = ctx.Categorias.ToList();
    return Results.Ok(categorias);
});


// Alterar uma categoria
// POST: http://localhost:5187/api/categoria/alterar 

app.MapPost("/api/categoria/alterar", ([FromBody] Categoria categoriaAtualizada, [FromServices] AppDataContext ctx) => 
{
    var categoriaExistente  = ctx.Categorias.SingleOrDefault(c => c.Id == categoriaAtualizada.Id);
    if(categoriaExistente == null){
        return Results.NotFound("Categoria não encotrada!");
    }
    if(string.IsNullOrWhiteSpace(categoriaAtualizada.Nome)){
        return Results.BadRequest("O nome da categoria nao pode ser vazio");
    }
    categoriaExistente.Nome = categoriaAtualizada.Nome;
    ctx.SaveChanges();
    return Results.Ok(categoriaExistente);
});

// DELETAR UMA CATEGORIA
// DELETE: http://localhost:5187/api/categoria/deletar/{id}

    app.MapDelete("/api/categoria/deletar/{id}", ([FromRoute] int id, [FromSeervices] AppDataContext ctx) => 
    {
        var categoria - ctx.Categorias.SingleOrDefault(c => c.Id == id );
        if(categoria == null){
            return Results.NotFound("Categoria não encotrada!");
        }

        ctx.Categorias.Remove(categoria);
        ctx.SaveChanges();
        return Results.Ok("Categoria deletada comm sucesso!");
    });

// Adicionar um filme
// http://localhost:5187/api/filme/cadastrar
app.MapPost("/api/filme/cadastrar", ([FromBody] Filme filme, [FromServices] AppDataContext ctx) =>
{
    var filmeExistente = ctx.Filmes.FirstOrDefault(x => x.Nome == filme.Nome);
    if (filmeExistente != null)
    {
        return Results.BadRequest("Já existe um filme com o mesmo nome.");
    }
    
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
app.MapGet("/api/filme/listar", ([FromServices] AppDataContext ctx) =>
{
    // MOSTRAR MENSAGEM CASO NAO HAJA FILMES CADASTRADOS
    return Results.Ok(ctx.Filmes.Include(f => f.Categoria).ToList());
});


// Listar filmes por categoria
// http://localhost:5187/api/filme/por-categoria/categoriaId
app.MapGet("/api/filme/por-categoria/categoriaId", ([FromRoute] int categoriaId, [FromServices] AppDataContext ctx) =>
{
    var filmes = ctx.Filmes.Include(f => f.Categoria).Where(f => f.CategoriaId == categoriaId).ToList();
    if(!filmes.Any()){
        return Results.NotFound("Nenhum filme encontrado nessa categoria!");
    }
    return Results.Ok(filmes);
});

// Buscar filme por nome
// http://localhost:5187/api/filme/buscar/nome
app.MapGet("/api/filme/buscar/nome", ([FromRoute] string nome, [FromServices] AppDataContext ctx) =>
{
    var filmes = ctx.Filmes.Include(f => f.Categoria).Where(f => f.Nome.Contains(nome)).ToList();
    if (!filmes.Any())
    {
        return Results.NotFound("Nenhum filme encontrado com esse nome.");
    }

    return Results.Ok(filmes);
});



// Alterar um filme
// POST: http://localhost:5187/api/filme/alterar
app.MapPost("/api/filme/alterar", ([FromBody] Filme filmeAtualizado, [FromServices] AppDataContext ctx) =>
{
    var filmeExistente = ctx.Filmes.SingleOrDefault(f => f.Id == filmeAtualizado.Id);
    if (filmeExistente == null)
    {
        return Results.NotFound("Filme não encontrado.");
    }

    if (string.IsNullOrWhiteSpace(filmeAtualizado.Nome))
    {
        return Results.BadRequest("O nome do filme invalido.");
    }

    filmeExistente.Nome = filmeAtualizado.Nome;
    // Atualize outros campos se necessário

    ctx.SaveChanges();
    return Results.Ok(filmeExistente);
});

// DELETAR UM FILME 
// DELETE: http://localhost:5187/api/filme/deletar/%7Bid%7D
app.MapDelete("/api/filme/deletar/{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
{
    var filme = ctx.Filmes.SingleOrDefault(f => f.Id == id);
    if (filme == null)
    {
        return Results.NotFound("Filme não encontrado.");
    }

    ctx.Filmes.Remove(filme);
    ctx.SaveChanges();
    return Results.Ok("Filme deletado!");
});


// Cadastrar uma sala
// POST: http://localhost:5187/api/sala/cadastrar
app.MapPost("/api/sala/cadastrar", ([FromBody] Sala sala, [FromServices] AppDataContext ctx) =>
{
    if (sala.AssentosOcupados > sala.QuantidadeAssentos)
    {
        return Results.BadRequest("Insira dados validos, você inseriu uma quantidade de assentos ocupados maior que a quantidade de assentos!");
    }

    if (sala.AssentosOcupados < 0 || sala.QuantidadeAssentos < 0)
    {
        return Results.BadRequest("Quantidade de assentos invalidas, não é possivel criar com 0 assentos!");
    }

    ctx.Salas.Add(sala);
    ctx.SaveChanges();
    return Results.Ok(sala);
});

// Listar todas as salas
// GET: http://localhost:5187/api/sala/listar
app.MapGet("api/sala/listar", ([FromServices] AppDataContext ctx) =>
{
    if (ctx.Salas.Any())
    {
        return Results.Ok(ctx.Salas.ToList());
    }
    return Results.NotFound("Nenhuma sala encontrada!");
});


app.Run();
