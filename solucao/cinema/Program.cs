using cinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();

builder.Services.AddCors(options =>
    options.AddPolicy("Acesso Total",
        configs => configs
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod())
);


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
    return Results.Ok(ctx.Categorias.ToList());
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
    return Results.Ok(ctx.Filmes.ToList());
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

    sala.AssentosOcupados = 0;

    if (sala.QuantidadeAssentos <= 0)
    {
        return Results.BadRequest("Quantidade de assentos inválida!");
    }

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
    return Results.Ok(ctx.Salas.ToList());
});

// CADASTRAR UMA SESSÃO
// POST: http://localhost:5187/api/sessao/cadastrar/
app.MapPost("/api/sessao/cadastrar", ([FromBody] Sessao sessao, [FromServices] AppDataContext ctx) =>
{
    if (string.IsNullOrEmpty(sessao.HorarioInicio) || string.IsNullOrEmpty(sessao.HorarioFinal))
    {
        return Results.BadRequest("Horário de início e final são obrigatórios!");
    }

    if (sessao.Dia <= 0 || sessao.Dia > 31)
    {
        return Results.BadRequest("Dia inválido!");
    }

    if (sessao.Mes <= 0 || sessao.Mes > 12)
    {
        return Results.BadRequest("Mês inválido!");
    }

    var filme = ctx.Filmes.FirstOrDefault(f => f.Id == sessao.FilmeId);
    if (filme == null)
    {
        return Results.NotFound("Filme não encontrado!");
    }

    var sala = ctx.Salas.FirstOrDefault(s => s.Id == sessao.SalaId);
    if (sala == null)
    {
        return Results.NotFound("Sala não encontrada!");
    }

    sessao.Filme = filme;
    sessao.Sala = sala;

    ctx.Sessoes.Add(sessao);
    ctx.SaveChanges();

    return Results.Created($"/api/sessao/{sessao.Id}", sessao);
});

// LISTAR TODAS SESSÃO
// POST: http://localhost:5187/api/sessao/listar/
app.MapGet("/api/sessao/listar", ([FromServices] AppDataContext ctx) =>
{
    if (!ctx.Sessoes.Any())
    {
        return Results.NotFound("Não foram encontradas sessões!");
    }

    var sessoes = ctx.Sessoes
        .Include(s => s.Filme)
            .ThenInclude(f => f.Categoria)
        .Include(s => s.Sala)
        .ToList();

    return Results.Ok(sessoes);
});


// LISTAR SESSÃO POR ID
// POST: http://localhost:5187/api/sessao/por-filme/{id}
app.MapGet("/api/sessao/por-filme/{id}", ([FromRoute] int id, [FromServices] AppDataContext ctx) =>
{
    var sessao = ctx.Sessoes.Include(s => s.Filme).Include(s => s.Sala).Where(s => s.FilmeId == id);

    if (sessao == null)
    {
        return Results.NotFound("Nenhuma sessão encontrada para esse filme.");
    }

    return Results.Ok(sessao);
});

// LISTAR SESSÃO POR DATA (MES/DIA)
// POST: http://localhost:5187/api/sessao/por-data/{mes}/{dia}
app.MapGet("/api/sessao/por-data/{mes}/{dia}", ([FromRoute] int mes, [FromRoute] int dia, [FromServices] AppDataContext ctx) =>
{
    var sessao = ctx.Sessoes
        .Include(s => s.Filme)
        .Include(s => s.Sala)
        .Where(s => s.Mes == mes && s.Dia == dia);

    if (sessao == null)
    {
        return Results.NotFound("Nenhuma sessão encontrada para essa data.");
    }

    return Results.Ok(sessao);
});

// ATUALIZAR SESSÃO
// PUT: http://localhost:5187/api/sessao/atualizar/{id}
app.MapPut("/api/sessao/atualizar/{id}", ([FromRoute] int id, [FromBody] Sessao sessaoAtualizada, [FromServices] AppDataContext ctx) =>
{
    var sessaoExistente = ctx.Sessoes.FirstOrDefault(s => s.Id == id);
    if (sessaoExistente == null)
    {
        return Results.NotFound("Sessão não encontrada.");
    }

    var filmeExistente = ctx.Filmes.FirstOrDefault(f => f.Id == sessaoAtualizada.FilmeId);
    if (filmeExistente == null)
    {
        return Results.NotFound("Filme não encontrado.");
    }

    var salaExistente = ctx.Salas.FirstOrDefault(s => s.Id == sessaoAtualizada.SalaId);
    if (salaExistente == null)
    {
        return Results.NotFound("Sala não encontrada.");
    }

    if (sessaoAtualizada.Dia < 1 || sessaoAtualizada.Dia > 31 || sessaoAtualizada.Mes < 1 || sessaoAtualizada.Mes > 12)
    {
        return Results.BadRequest("Dia ou mês de sessão inválidos.");
    }

    sessaoExistente.HorarioInicio = sessaoAtualizada.HorarioInicio;
    sessaoExistente.HorarioFinal = sessaoAtualizada.HorarioFinal;

    sessaoExistente.Dia = sessaoAtualizada.Dia;
    sessaoExistente.Mes = sessaoAtualizada.Mes;

    sessaoExistente.FilmeId = sessaoAtualizada.FilmeId;
    sessaoExistente.Filme = filmeExistente;

    sessaoExistente.SalaId = sessaoAtualizada.SalaId;
    sessaoExistente.Sala = salaExistente;

    ctx.SaveChanges();
    return Results.Ok(sessaoExistente);
});

// DELETAR SESSÃO
// POST: http://localhost:5187/api/sessao/excluir/{sessaoId}
app.MapDelete("/api/sessao/excluir/{sessaoId}", ([FromRoute] int sessaoId, [FromServices] AppDataContext ctx) =>
{
    var sessao = ctx.Sessoes.FirstOrDefault(s => s.Id == sessaoId);
    if (sessao == null)
    {
        return Results.NotFound("Sessão não encontrada.");
    }

    ctx.Sessoes.Remove(sessao);
    ctx.SaveChanges();

    return Results.Ok("Sessão removida com sucesso.");
});

// Cadastrar uma reserva
// POST: http://localhost:5187/api/reserva/cadastrar
app.MapPost("/api/reserva/cadastrar", ([FromBody] Reserva reserva, [FromServices] AppDataContext ctx) =>
{
    foreach (var reservaSessao in reserva.ReservaSessoes)
    {
        var sessao = ctx.Sessoes.FirstOrDefault(s => s.Id == reservaSessao.SessaoId);
        if (sessao == null)
        {
            return Results.NotFound($"Sessão com Id {reservaSessao.SessaoId} não encontrada!");
        }
        reservaSessao.Sessao = sessao;
    }

    // Validar se há sessões na reserva
    if (reserva.ReservaSessoes == null || reserva.ReservaSessoes.Count == 0)
    {
        return Results.BadRequest("A reserva deve conter pelo menos uma sessão.");
    }
    
    // Validar se todas as sessões da reserva existem
    foreach (var reservaSessao in reserva.ReservaSessoes)
    {
        var sessao = ctx.Sessoes.FirstOrDefault(s => s.Id == reservaSessao.SessaoId);
        if (sessao == null)
        {
            return Results.NotFound($"Sessão com Id {reservaSessao.SessaoId} não encontrada!");
        }
        reservaSessao.Sessao = sessao;
    }

    ctx.Reservas.Add(reserva);
    ctx.SaveChanges();
    return Results.Created($"/api/reserva/{reserva.Id}", reserva);
});

// Listar todas as reservas
// GET: http://localhost:5187/api/reserva/listar
app.MapGet("/api/reserva/listar", ([FromServices] AppDataContext ctx) =>
{
    var reservas = ctx.Reservas
        .Include(r => r.ReservaSessoes)
            .ThenInclude(rs => rs.Sessao)
        .ToList();

 // Verifica se não há reservas encontradas
    if (reservas == null || reservas.Count == 0)
    {
        
        return Results.NotFound("Não foram encontradas reservas.");
    }
    
    return Results.Ok(reservas);
});

app.UseCors("Acesso Total");

app.Run();