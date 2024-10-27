using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<HeroesContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () =>
{
    return Results.Text("Welcome to the Tour of Heroes API", "text/html");
});

app.MapGet("/api/hero", async (HeroesContext db) =>
{
    return await db.Heroes.ToListAsync();
})
.WithName("GetHeroes")
.WithOpenApi();

app.MapPost("/api/hero", async (HeroesContext db, Hero hero) =>
{
    db.Heroes.Add(hero);
    await db.SaveChangesAsync();
    return Results.Created($"/api/hero/{hero.Id}", hero);
});

app.Run();

public record Hero(int Id, string Name, string AlterEgo, string Description);