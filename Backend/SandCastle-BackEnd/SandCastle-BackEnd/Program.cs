using MongoDB.Driver;
using SandCastle_BackEnd.MongoRelated;

// Configura��es de Build
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
app.MapGet("/Fichas", async () => await fichaContext.Fichas.FindAsync(p => true));

// Pega ficha espec�fica
app.MapGet("/Fichas/{name}", async (string id) => await fichaContext.Fichas.FindAsync(p => p.Id == id));

// Cria nova Ficha
app.MapPost("/Fichas", async (Ficha insert) => await fichaContext.Fichas.InsertOneAsync(insert));

// Atualiza uma ficha espec�fica
app.MapPut("/Fichas/", async (Ficha update) => await fichaContext.Fichas.ReplaceOneAsync(f => f.Id == update.Id, update));

// Deleta uma ficha espec�fica
app.MapDelete("/Fichas/{id}", (string id) => fichaContext.Fichas.FindOneAndDelete(p => p.Id == id));

app.UseSwaggerUI();

app.Run();