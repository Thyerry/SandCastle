using Microsoft.EntityFrameworkCore;

// Configurações de Build
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("Fichas"));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();

// Endpoints
app.MapGet("/Fichas", async (AppDbContext context) => await context.Fichas.ToListAsync());

app.MapGet("/Fichas/{id}", async (int id, AppDbContext context) => await context.Fichas.FirstOrDefaultAsync(f => f.Id == id));

app.MapPost("/Fichas", async (Ficha insert, AppDbContext context) =>
{
    context.Fichas.Add(insert);
    await context.SaveChangesAsync();

    return insert;
});

app.MapPut("/Fichas/{id}", async (Ficha update, AppDbContext context) =>
{
    context.Entry(update).State = EntityState.Modified;
    await context.SaveChangesAsync();
    return update;
});

app.MapDelete("/Fichas/{id}", async (int id, AppDbContext context) =>
{
    var result = await context.Fichas.FirstOrDefaultAsync(a => a.Id == id);

    if (result != null)
    {
        context.Remove(result);
        await context.SaveChangesAsync();
    }
});

app.UseSwaggerUI();

app.Run();