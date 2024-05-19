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
    if (ctx.Categorias.Any(c => c.Nome == categoria.Nome))
    {
        return Results.BadRequest("Já existe uma categoria com esse nome!");
    }

    ctx.Categorias.Add(categoria);
    ctx.SaveChanges();
    return Results.Created("Categoria criada!", categoria);

});

// Listar uma categoria
// http://localhost:5187/api/categoria/listar
app.MapGet("/api/categoria/listar", ([FromServices] AppDataContext ctx) =>
{
    var categorias = ctx.Categorias.ToList();
    if (!categorias.Any())
    {
        return Results.NotFound("Nenhuma categoria cadastrada!");
    }
    return Results.Ok(categorias);


});


// Alterar uma categoria
// PUT: http://localhost:5187/api/categoria/alterar 

app.MapPut("/api/categoria/alterar/{id}", ([FromBody] Categoria categoriaAtualizada, [FromRoute] int id, [FromServices] AppDataContext ctx) =>
{
    var categoriaExistente = ctx.Categorias.FirstOrDefault(c => c.Id == id);
    if (categoriaExistente == null)
    {
        return Results.NotFound("Categoria com esse id não encotrada!");
    }
    if (ctx.Categorias.Any(c => c.Nome == categoriaAtualizada.Nome))
    {
        return Results.BadRequest("Já existe uma categoria com esse nome!");
    }
    if (string.IsNullOrWhiteSpace(categoriaAtualizada.Nome))
    {
        return Results.BadRequest("O nome da categoria não pode ser vazio");
    }
    categoriaExistente.Nome = categoriaAtualizada.Nome;
    ctx.SaveChanges();
    return Results.Ok(categoriaExistente);
});

// DELETAR UMA CATEGORIA
// DELETE: http://localhost:5187/api/categoria/deletar/{id}

app.MapDelete("/api/categoria/deletar/{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
{
    var categoria = ctx.Categorias.FirstOrDefault(c => c.Id == id);
    if (categoria == null)
    {
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
    if (filme.Duracao <= 0)
    {
        return Results.BadRequest("Duração de filme invalido!");
    }
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
    if (!ctx.Filmes.Any())
    {
        return Results.NotFound("Não foi encontrado nenhum filme!");
    }
    return Results.Ok(ctx.Filmes.Include(f => f.Categoria).Include(f => f.Sessao).ToList());
});


// Listar filmes por categoria
// http://localhost:5187/api/filme/por-categoria/categoriaId
app.MapGet("/api/filme/por-categoria/{categoriaId}", ([FromRoute] int categoriaId, [FromServices] AppDataContext ctx) =>
{
    if (!ctx.Categorias.Any(f => f.Id == categoriaId))
    {
        return Results.NotFound("Não foi encontrada nenhuma categoria com esse id!");
    }
    var filmes = ctx.Filmes.Include(f => f.Categoria).Where(f => f.CategoriaId == categoriaId).ToList();
    if (!filmes.Any())
    {
        return Results.NotFound("Nenhum filme encontrado nessa categoria!");
    }
    return Results.Ok(filmes);
});

// Buscar filme por nome
// http://localhost:5187/api/filme/buscar/nome
app.MapGet("/api/filme/buscar/{nome}", ([FromRoute] string nome, [FromServices] AppDataContext ctx) =>
{
    var filmes = ctx.Filmes.Include(f => f.Categoria).Where(f => f.Nome.Contains(nome)).ToList();
    if (!filmes.Any())
    {
        return Results.NotFound("Nenhum filme encontrado com esse nome.");
    }

    return Results.Ok(filmes);
});

// Alterar um filme
// PUT: http://localhost:5187/api/filme/alterar
app.MapPut("/api/filme/alterar/{id}", ([FromBody] Filme filmeAtualizado, [FromRoute] int id, [FromServices] AppDataContext ctx) =>
{
    var filmeExistente = ctx.Filmes.FirstOrDefault(f => f.Id == id);
    if (filmeExistente == null)
    {
        return Results.NotFound("Filme não encontrado.");
    }

    if (string.IsNullOrWhiteSpace(filmeAtualizado.Nome))
    {
        return Results.BadRequest("O nome do filme é inválido.");
    }


    filmeExistente.Nome = filmeAtualizado.Nome;
    if (filmeAtualizado.Duracao <= 0)
    {
        return Results.BadRequest("Duração invalida!");
    }

    filmeExistente.Duracao = filmeAtualizado.Duracao;

    var categoriaExistente = ctx.Categorias.FirstOrDefault(c => c.Id == filmeAtualizado.CategoriaId);
    if (categoriaExistente == null)
    {
        return Results.BadRequest("Categoria não encontrada.");
    }

    filmeExistente.CategoriaId = filmeAtualizado.CategoriaId;
    filmeExistente.Categoria = categoriaExistente;

    if (filmeAtualizado.Sessao != null && filmeExistente.Sessao != null)
    {
        var sessaoExistente = ctx.Sessoes.FirstOrDefault(s => s.Id == filmeExistente.Sessao.Id);

        if (sessaoExistente == null)
        {
            return Results.NotFound("Sessão não encontrada!");
        }

        sessaoExistente.HorarioInicio = filmeAtualizado.Sessao.HorarioInicio;
        sessaoExistente.HorarioFinal = filmeAtualizado.Sessao.HorarioFinal;
        sessaoExistente.Dia = filmeAtualizado.Sessao.Dia;
        sessaoExistente.Mes = filmeAtualizado.Sessao.Mes;
        sessaoExistente.SalaId = filmeAtualizado.Sessao.SalaId;
    }

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

    if (sala.AssentosOcupados < 0 || sala.QuantidadeAssentos <= 0)
    {
        return Results.BadRequest("Quantidade de assentos invalidas!");
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

// EDITAR UMA SALA 
// PUT: http://localhost:5187/api/sala/alterar/{id}
app.MapPut("/api/sala/alterar/{id}", ([FromBody] Sala salaAtualizada, [FromRoute] int id, [FromServices] AppDataContext ctx) =>
{
    var salaExistente = ctx.Salas.FirstOrDefault(s => s.Id == id);
    if (salaExistente == null)
    {
        return Results.NotFound("Sala não encontrada!");
    }

    if (salaAtualizada.AssentosOcupados > salaAtualizada.QuantidadeAssentos)
    {
        return Results.BadRequest("Numero de assentos ocupados maior que a quantidade de assentos!");
    }

    if (salaAtualizada.AssentosOcupados < 0 || salaAtualizada.QuantidadeAssentos < 0)
    {
        return Results.BadRequest("Numero de assentos inválidas!");
    }


    if (salaAtualizada.AssentosOcupados < 0 || salaAtualizada.QuantidadeAssentos <= 0)
    {
        return Results.BadRequest("Quantidade de assentos invalidas!");
    }
    salaExistente.QuantidadeAssentos = salaAtualizada.QuantidadeAssentos;
    salaExistente.AssentosOcupados = salaAtualizada.AssentosOcupados;

    ctx.SaveChanges();
    return Results.Ok(salaExistente);
});

// DELETAR UMA SALA 
// DELETE: http://localhost:5187/api/sala/excluir/{id}
app.MapDelete("/api/sala/excluir/{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
{
    var salaParaExcluir = ctx.Salas.FirstOrDefault(s => s.Id == id);
    if (salaParaExcluir == null)
    {
        return Results.NotFound("Sala não encontrada!");
    }

    ctx.Salas.Remove(salaParaExcluir);
    ctx.SaveChanges();
    return Results.Ok("Sala excluída!");
});



app.Run();
