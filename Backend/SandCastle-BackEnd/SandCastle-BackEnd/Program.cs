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

// Cria nova Ficha
app.MapPost("/Fichas", (Func<Ficha, Task<dynamic>>)(async insert =>
{
    try
    {
        await mongoContext.Fichas.InsertOneAsync(insert);

        var jogo = (await mongoContext.Jogos.FindAsync(j => j.Id == insert.IdJogo)).FirstOrDefault();

        if (jogo != null)
        {
            if (jogo.Fichas != null)
                jogo.Fichas.Add(insert.Id);
            else
                jogo.Fichas = new List<string> { insert.Id };

            await mongoContext.Jogos.ReplaceOneAsync(j => j.Id == jogo.Id, jogo);
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

app.MapPost("/Jogo", (Func<Jogo, Task<dynamic>>) (async insert =>
{
    try
    {
        await mongoContext.Jogos.InsertOneAsync(insert);

        if (insert.Fichas != null)
        {
            foreach (var item in insert.Fichas)
            {
                var jogador = (await mongoContext.Fichas.FindAsync(j => j.Id == item)).FirstOrDefault();

                if(jogador != null)
                {
                    jogador.IdJogo = insert.Id;

                    await mongoContext.Fichas.ReplaceOneAsync(j => j.Id == jogador.Id, jogador);
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
    var player = (await mongoContext.Fichas.FindAsync(j => j.Id == idJogador)).FirstOrDefault();

    if (update.Fichas == null)
        update.Fichas = new List<string>();

    if (player == null)
        throw new Exception("Não existe jogador com esse id");

    update.Fichas.Add(player.Id);

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