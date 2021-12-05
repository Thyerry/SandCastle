using MongoDB.Driver;
using SandCastle_BackEnd.Entidades;
using SandCastle_BackEnd.MongoRelated;
using System.Net;

// Configurações de Build
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => 
{
    options.AddPolicy(name: "_myAllowAnyOrigins",
                      builder =>
                      {
                          builder.AllowAnyOrigin();
                      });
});

var mongoContext = new MongoContext(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseCors();
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

// Pega fichas de um jogo específico
app.MapGet("/Fichas/Jogo/{id}", (Func<string, Task<dynamic>>)(async idJogo =>
{
    try
    {
        var result = (await mongoContext.Fichas.FindAsync(p => p.IdJogo == idJogo)).ToList();

        if (!result.Any())
            return new ErroReturn { Erro = "Não existem fichas cadastradas para este jogo." };


        return result.ToList();
    }
    catch (Exception)
    {
        return new ErroReturn { Erro = "Id invalido" };
    }
}));

// Pega ficha de um jogador específico
app.MapGet("/Fichas/Jogador/{id}", (Func<string, Task<dynamic>>)(async idJogador =>
{
    try
    {
        var result = (await mongoContext.Fichas.FindAsync(p => p.IdJogador == idJogador)).ToList();

        if (!result.Any())
            return new ErroReturn { Erro = "Não existem fichas cadastradas para este jogador." };

        return result.ToList();
    }
    catch (Exception)
    {
        return new ErroReturn { Erro = "Id invalido" };
    }
}));

// Cria nova Ficha
app.MapPost("/Fichas", (Func<Ficha, Task<dynamic>>)(async insert =>
{
    try
    {
        await mongoContext.Fichas.InsertOneAsync(insert);

        if (insert.Id != null)
        {
            var jogador = (await mongoContext.Jogadores.FindAsync(j => j.Id == insert.Id)).FirstOrDefault();

            if (jogador != null)
            {
                if (jogador.Fichas != null)
                    jogador.Fichas.Add(insert.Id);
                else
                    jogador.Fichas = new List<string> { insert.Id };

                await mongoContext.Jogadores.ReplaceOneAsync(j => j.Id == jogador.Id, jogador);
            }
        }
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

app.MapGet("/Jogador", (Func<Task<dynamic>>) (async () =>
{
    try
    {
        var result = (await mongoContext.Jogadores.FindAsync(p => true)).ToList();

        if (!result.Any())
            return new ErroReturn { Erro = "não existem jogadores cadastradas" };

        return result;
    }
    catch (Exception)
    {
        return new ErroReturn { Erro = "id no formato invalido" };
    }
}));

app.MapGet("/Jogador/{id}", (Func<string, Task<dynamic>>)(async id =>
{
    try
    {
        var result = (await mongoContext.Jogadores.FindAsync(p => p.Id == id)).FirstOrDefault();

        if (result == null)
            return new ErroReturn { Erro = $"Não existe jogadore cadastrado no id {id}" };

        return result;
    }
    catch (Exception)
    {
        return new ErroReturn { Erro = "erro: id invalido" };
    }
}));

app.MapGet("/Jogador/Jogos/{id}", (Func<string, Task<dynamic>>)(async id =>
{
    try
    {
        var idJogadores = (await mongoContext.Jogos.FindAsync(p => p.Id == id)).FirstOrDefault();
        var result = new List<Jogador>();

        foreach (var idJogador in idJogadores.Jogadores)
            result.Add(mongoContext.Jogadores.Find(j => j.Id == idJogador).FirstOrDefault());

        if (!result.Any())
            return new ErroReturn { Erro = $"Não existe jogadores cadastrados no jogo de id {id}" };

        return result;
    }
    catch (NullReferenceException e)
    {
        return new ErroReturn { Erro = "o jogador não está cadastrado em nenhum jogo!" };
    }
    catch (Exception)
    {
        return new ErroReturn { Erro = "id invalido" };
    }
}));

app.MapPost("/Jogador", (Func<Jogador, Task<dynamic>>)(async insert =>
{
    try
    {
        await mongoContext.Jogadores.InsertOneAsync(insert);
    }
    catch (Exception e)
    {
        return new ErroReturn { Erro = $"erro: Não foi possível criar a jogo! Detalhes do erro: {e.Message}" };
    }

    return HttpStatusCode.Created;
}));

app.MapPut("/Jogador/", (Func<Jogador, Task<dynamic>>)(async update =>
{
    try
    {
        var result = (await mongoContext.Jogadores.FindAsync(j => j.Id == update.Id)).FirstOrDefault();

        if (result == null)
            return new ErroReturn { Erro = $"não existe jogo cadastrado no id {update.Id}" };

        return await mongoContext.Jogadores.ReplaceOneAsync(f => f.Id == update.Id, update);
    }
    catch (Exception)
    {
        return new ErroReturn { Erro = "erro: id invalido" };
    }
}));

app.MapDelete("/Jogador/{id}", (Func<string, Task<dynamic>>)(async id =>
{
    try
    {
        var result = await mongoContext.Jogadores.FindOneAndDeleteAsync(p => p.Id == id);
        return result;
    }
    catch (Exception)
    {
        return new ErroReturn { Erro = "erro: id invalido" };
    }
}));
#endregion Jogador

#region Jogo

app.MapGet("/Jogo", (Func<Task<dynamic>>) (async () =>
{
    try
    {
        var result = (await mongoContext.Jogos.FindAsync(p => true)).ToList();

        if (!result.Any())
            return new ErroReturn { Erro = "não existem jogos cadastradas" };

        return result;
    }
    catch (Exception)
    {
        return new ErroReturn { Erro = "id no formato invalido" };
    }

}));

app.MapGet("/Jogo/{id}", (Func<string, Task<dynamic>>)(async id =>
{
    try
    {
        var result = (await mongoContext.Jogos.FindAsync(p => p.Id == id)).FirstOrDefault();

        if(result == null)
            return new ErroReturn { Erro = $"Não existe jogo cadastrado no id {id}" };

        return result;
    }
    catch (Exception)
    {
        return new ErroReturn { Erro = "erro: id invalido" };
    }
}));

app.MapGet("/Jogo/Jogador/{idJogador}", (Func<string, Task<dynamic>>)(async idJogador =>
{
    try
    {
        var result = (await mongoContext.Jogadores.FindAsync(p => p.Id == idJogador)).FirstOrDefault();
        if (result == null)
            return new ErroReturn { Erro = $"Não existe jogador cadastrado no id {idJogador}" };

        var jogos = new List<Jogo>();

        foreach (var item in result.Jogos)
            jogos.Add(mongoContext.Jogos.Find(j => j.Id == item).FirstOrDefault());

        return result;
    }
    catch (Exception)
    {
        return new ErroReturn { Erro = "erro: id invalido" };
    }
}));

app.MapPost("/Jogo", (Func<Jogo, Task<dynamic>>) (async insert =>
{
    try
    {
        await mongoContext.Jogos.InsertOneAsync(insert);

        if (insert.Jogadores != null || insert.Jogadores.Any())
        {
            foreach (var item in insert.Jogadores)
            {
                var jogador = (await mongoContext.Jogadores.FindAsync(j => j.Id == item)).FirstOrDefault();

                if(jogador != null)
                {
                    if (jogador.Jogos != null)
                        jogador.Jogos.Add(insert.Id);
                    else
                        jogador.Jogos = new List<string> { insert.Id };

                    await mongoContext.Jogadores.ReplaceOneAsync(j => j.Id == jogador.Id, jogador);
                }
            }
        }
    }
    catch (Exception e)
    {
        return new ErroReturn { Erro = $"erro: Não foi possível criar a jogo! Detalhes do erro: {e.Message}" };
    }

    return HttpStatusCode.Created;
}));

app.MapPut("/Jogo/", (Func<Jogo, Task<dynamic>>) (async  update =>
{
    try
    {
        var result = (await mongoContext.Jogos.FindAsync(j => j.Id == update.Id)).FirstOrDefault(); 

        if (result == null)
            return new ErroReturn { Erro = $"não existe jogo cadastrado no id {update.Id}" };

        return await mongoContext.Jogos.ReplaceOneAsync(f => f.Id == update.Id, update);
    }
    catch (Exception)
    {
        return new ErroReturn { Erro = "erro: id invalido" };
    }
}));

app.MapPut("/Jogo/{idJogo}/{idJogador}", async (string idJogo, string idJogador) =>
{
    var update = (await mongoContext.Jogos.FindAsync(j => j.Id == idJogo)).FirstOrDefault();
    var player = (await mongoContext.Jogadores.FindAsync(j => j.Id == idJogador)).FirstOrDefault();

    if (update.Jogadores == null)
        update.Jogadores = new List<string>();

    if (player == null)
        throw new Exception("Não existe jogador com esse id");

    update.Jogadores.Add(player.Id);
    player.Jogos.Add(update.Id);
    var result = await mongoContext.Jogos.ReplaceOneAsync(f => f.Id == update.Id, update);

    return update;
});

app.MapDelete("/Jogo/{id}", (Func<string, Task<dynamic>>)(async id =>
{
    try
    {
        var result = await mongoContext.Jogos.FindOneAndDeleteAsync(p => p.Id == id);

        return result;
    }
    catch (Exception)
    { 
        return new ErroReturn { Erro = "erro: id invalido" };
    }
}));

#endregion Jogo

app.UseSwaggerUI();

app.Run();

public class ErroReturn
{
    public string Erro { get; set; }
}