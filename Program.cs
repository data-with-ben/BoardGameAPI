using Microsoft.EntityFrameworkCore;
using BoardGameAPI.DTOs.Requests;
using BoardGameAPI.Configuration;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BoardGameDb>(opt => opt.UseInMemoryDatabase("BoardGameAPI"));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "A Board Games Knight";
    config.Title = "BoardKnight v1";
    config.Version = "v1";
    config.PostProcess = SwaggerExamples.AddExamples;
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "A Board Games Knight";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.MapGet("/BoardGames", async (BoardGameDb db) =>
    await db.BoardGames.ToListAsync());

app.MapGet("/BoardGame/{slug}", async (string slug, BoardGameDb db) =>
{
    var bg = await db.BoardGames.FirstOrDefaultAsync(bg => Slugifier.GenerateSlug(bg.Slug) == slug);
    var boardGame = new BoardGame()
    {
        Name = bg.Name,
        Description = bg.Description,
        Slug = bg.Slug
    };
    return Results.Ok(boardGame); 
});


app.MapPost("/BoardGame", async (CreateBoardGameRequest request, BoardGameDb db) =>
{
    var boardGame = new BoardGame 
    { 
        Name = request.Name,
        Description = request.Description,
        Slug = Slugifier.GenerateSlug(request.Name)
    };
    
    db.BoardGames.Add(boardGame);
    await db.SaveChangesAsync();

    return Results.Created($"/BoardGame/{boardGame.Id}", boardGame);
});

app.MapPut("/BoardGame/{slug}", async (string slug, UpdateBoardGameRequest inputBoardGame, BoardGameDb db) =>
{
    var BoardGame = await db.BoardGames.FirstOrDefaultAsync(b => b.Slug == slug);

    if (BoardGame is null) return Results.NotFound();

    BoardGame.Name = inputBoardGame.Name;
    BoardGame.Slug = Slugifier.GenerateSlug(inputBoardGame.Name);
    BoardGame.Description = inputBoardGame.Description;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/BoardGame/{slug}", async (string slug, BoardGameDb db) =>
{
    if (await db.BoardGames.FindAsync(slug) is BoardGame BoardGame)
    {
        db.BoardGames.Remove(BoardGame);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();