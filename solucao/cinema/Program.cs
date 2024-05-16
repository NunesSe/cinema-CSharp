using cinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();
var app = builder.Build();

//Cadastrar uma categoria
// http://localhost:5187/api/categoria/cadastrar
app.MapPost("/api/categoria/cadastrar", ([FromBody] Categoria categoria, [FromServices] AppDataContext ctx) =>{
    ctx.Categorias.Add(categoria);
    ctx.SaveChanges();
    return Results.Created("Categoria criada!", categoria);

}); 

//Listar uma categoria
// http://localhost:5187/api/categoria/listar
app.MapGet("/api/categoria/listar",([FromServices] AppDataContext ctx ) =>{
    return Results.Ok(ctx.Categorias.ToList());
});

app.Run();
