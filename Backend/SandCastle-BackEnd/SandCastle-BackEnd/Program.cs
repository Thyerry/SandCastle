using MongoDB.Driver;
using SandCastle_BackEnd.Entidades;
using SandCastle_BackEnd.MongoRelated;
using System.Net;

// Configurações de Build
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mongoContext = new MongoContext(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();

#region Fichas

// Pega todas as Fichas
app.MapGet("/Fichas", (Func<Task<dynamic>>)(async () =>
{
    var result = (await mongoContext.Fichas.FindAsync(p => true)).ToList();

    if (!result.Any())
        return new ErroReturn { Erro = "erro: não existem fichas cadastradas"};

    return result;
}));

// Pega ficha específica
app.MapGet("/Fichas/{id}", (Func<string, Task<dynamic>>)(async id =>
{
    try
    {
        var result =  await mongoContext.Fichas.FindAsync(p => p.Id == id);
        return result.FirstOrDefault();
    }
    catch (Exception)
    {
        return new ErroReturn { Erro = "erro: id invalido" };
    }
}));

// Cria nova Ficha
app.MapPost("/Fichas", (Func<Ficha, Task<dynamic>>)(async insert =>
{
    try
    {
        await mongoContext.Fichas.InsertOneAsync(insert);
    }
    catch (Exception e)
    {
        return new ErroReturn { Erro = $"erro: Não foi possível criar a ficha! Detalhes do erro: {e.Message}" };
    }

    return HttpStatusCode.Created;
}));

// Atualiza uma ficha específica
app.MapPut("/Fichas/", (Func<Ficha, Task<dynamic>>)(async update =>
{
    try
    {
        var result = await mongoContext.Fichas.ReplaceOneAsync(f => f.Id == update.Id, update);
    }
    catch (Exception)
    {
        return new ErroReturn { Erro = "erro: id invalido" };
    }
    return update;
}));

// Deleta uma ficha específica
app.MapDelete("/Fichas/{id}", (Func<string, Task<dynamic>>)(async id =>
{
    try
    {
        var result = await mongoContext.Fichas.FindOneAndDeleteAsync(p => p.Id == id);
        return result;
    }
    catch (Exception)
    { 
        return new ErroReturn { Erro = "erro: id invalido" };
    }
}));

#endregion Fichas

#region Jogador

app.MapGet("/Jogador", async () =>
{
    var result = await mongoContext.Jogadores.FindAsync(p => true);
    return result.ToList();
});

app.MapGet("/Jogador/{id}", async (string id) =>
{
    var result = await mongoContext.Jogadores.FindAsync(p => p.Id == id);
    return result.FirstOrDefault();
});

app.MapPost("/Jogador", async (Jogador insert) =>
{
    await mongoContext.Jogadores.InsertOneAsync(insert);
    return HttpStatusCode.Created;
});

app.MapPut("/Jogador/", async (Jogador update) =>
{
    var result = await mongoContext.Jogadores.ReplaceOneAsync(f => f.Id == update.Id, update);
    return update;
});

app.MapDelete("/Jogador/{id}", (string id) => mongoContext.Jogadores.FindOneAndDelete(p => p.Id == id));

#endregion Jogador

#region Jogo

app.MapGet("/Jogo", async () =>
{
    var result = await mongoContext.Jogos.FindAsync(p => true);
    return result.ToList();
});

app.MapGet("/Jogo/{id}", async (string id) =>
{
    var result = await mongoContext.Jogos.FindAsync(p => p.Id == id);
    return result.FirstOrDefault();
});

app.MapPost("/Jogo", async (Jogo insert) =>
{
    await mongoContext.Jogos.InsertOneAsync(insert);
    return HttpStatusCode.Created;
});

app.MapPut("/Jogo/", async (Jogo update) =>
{
    var result = await mongoContext.Jogos.ReplaceOneAsync(f => f.Id == update.Id, update);
    return update;
});

app.MapPut("/Jogo/{idJogo}/{idJogador}", async (string idJogo, string idJogador) =>
{
    var update = (await mongoContext.Jogos.FindAsync(j => j.Id == idJogo)).FirstOrDefault();
    var player = (await mongoContext.Jogadores.FindAsync(j => j.Id == idJogador)).FirstOrDefault();

    if (update.Jogadores == null)
        update.Jogadores = new List<string>();

    if (player == null)
        throw new Exception("Não existe jogador com esse id");

    update.Jogadores.Add(player.Id);

    var result = await mongoContext.Jogos.ReplaceOneAsync(f => f.Id == update.Id, update);
    return update;
});

app.MapDelete("/Jogo/{id}", (string id) => mongoContext.Jogos.FindOneAndDelete(p => p.Id == id));

#endregion Jogo

app.UseSwaggerUI();

app.Run();




public class ErroReturn
{
    public string Erro { get; set; }
}