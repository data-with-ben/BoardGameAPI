using Microsoft.EntityFrameworkCore;
using BoardGameAPI.DTOs.Requests;
using BoardGameAPI.Configuration;
using BoardGameAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<GameKnightDb>(opt => opt.UseInMemoryDatabase("GameKnightDb"));
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

app.MapGet("/BoardGames", async ([FromServices] GameKnightDb db) =>
    await db.BoardGames.ToListAsync());

app.MapGet("/BoardGame/{slug}", async (string slug, [FromServices] GameKnightDb db) =>
{
    if(await db.BoardGames.FirstOrDefaultAsync(bg => Slugifier.GenerateSlug(bg.Slug) == slug) is BoardGame boardGame)
    {
        return Results.Ok(boardGame);   
    }
    return Results.NotFound();
});

app.MapGet("/User/{Email}", async (string email, [FromServices] GameKnightDb db) =>
{
    if(await db.Users.Include(u => u.BoardGames).FirstOrDefaultAsync(bg => bg.Email == email) is User user)
    {
        return Results.Ok(user);
    }
    return Results.NotFound();
});


app.MapPost("/BoardGame", async ([FromBody] CreateBoardGameRequest request, [FromServices] GameKnightDb db) =>
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

app.MapPost("/User", async ([FromBody] CreateUserRequest request, [FromServices] GameKnightDb db) => 
{
    var user = new User
    {
        Handle = request.Handle,
        Email = request.Email,
        FirstName = request.FirstName,
        LastName = request.LastName
    };
    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Created($"/User/{user.Email}", user);
});

app.MapPost("/User/AddBoardGame", async ([FromBody] CreateUserBoardGameRequest request, [FromServices] GameKnightDb GameKnightDb) =>
{
    var user = await GameKnightDb.Users
        .Include(u => u.BoardGames)
        .FirstOrDefaultAsync(u => u.Email == request.Email);
    
    if (user == null)
        return Results.NotFound("User not found!");

    var boardGame = await GameKnightDb.BoardGames
        .FirstOrDefaultAsync(b => b.Slug == request.BoardGame.Slug);
    
    if (boardGame == null)
        return Results.NotFound("Board game not found!");

    user.BoardGames.Add(boardGame);
    await GameKnightDb.SaveChangesAsync();

    return Results.Ok(user);
});

app.MapPut("/BoardGame/{slug}", async (string slug, [FromBody] UpdateBoardGameRequest inputBoardGame, [FromServices] GameKnightDb db) =>
{
    var BoardGame = await db.BoardGames.FirstOrDefaultAsync(b => b.Slug == slug);

    if (BoardGame is null) return Results.NotFound();

    BoardGame.Name = inputBoardGame.Name;
    BoardGame.Slug = Slugifier.GenerateSlug(inputBoardGame.Name);
    BoardGame.Description = inputBoardGame.Description;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/BoardGame/{slug}", async (string slug, [FromServices] GameKnightDb db) =>
{
    if (await db.BoardGames.FirstOrDefaultAsync(x => x.Slug == slug) is BoardGame BoardGame)
    {
        db.BoardGames.Remove(BoardGame);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();