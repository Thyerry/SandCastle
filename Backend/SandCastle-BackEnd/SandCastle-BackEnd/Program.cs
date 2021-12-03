using MongoDB.Driver;
using SandCastle_BackEnd.MongoRelated;
using System.Net;

// Configurações de Build
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var fichaContext = new FichaContext(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();

// Endpoints
// Pega todas as Fichas
app.MapGet("/Fichas", async () =>
{
    var result = await fichaContext.Fichas.FindAsync(p => true);
    return result.ToList();
});

// Pega ficha específica
app.MapGet("/Fichas/{id}", async (string id) => 
{ 
    var result = await fichaContext.Fichas.FindAsync(p => p.Id == id);
    return result.FirstOrDefault();
});

// Cria nova Ficha
app.MapPost("/Fichas", async (Ficha insert) =>
{
    await fichaContext.Fichas.InsertOneAsync(insert);
    return HttpStatusCode.Created;
});

// Atualiza uma ficha específica
app.MapPut("/Fichas/", async (Ficha update) =>
{
    var result = await fichaContext.Fichas.ReplaceOneAsync(f => f.Id == update.Id, update);
    return result;
});

// Deleta uma ficha específica
app.MapDelete("/Fichas/{id}", (string id) => fichaContext.Fichas.FindOneAndDelete(p => p.Id == id));

app.UseSwaggerUI();

app.Run();