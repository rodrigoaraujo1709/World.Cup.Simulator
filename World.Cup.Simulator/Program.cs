using Microsoft.EntityFrameworkCore;
using World.Cup.Simulator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WorldCupContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ServerConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/teams/{id}", async (WorldCupContext context, Guid id) =>
{
    var team = await context.Teams.FindAsync(id);

    return Results.Ok(team);
});

app.MapGet("/teams", async (WorldCupContext context) =>
{
    var teams = await context.Teams.ToListAsync();

    return Results.Ok(teams);
});

app.MapPost("/teams", async (WorldCupContext context, Team team) =>
{
    await context.Teams.AddAsync(team);
    await context.SaveChangesAsync();

    return Results.Ok(team);
});

app.MapPut("/teams/{id}", async (WorldCupContext context, Team team) =>
{
    var dbTeam = await context.Teams.FindAsync(team.Id);
    if (dbTeam == null)
        return Results.NotFound();

    dbTeam.Name = team.Name;
    dbTeam.Img = team.Img;

    context.Teams.Update(dbTeam);
    await context.SaveChangesAsync();

    return Results.Ok(dbTeam);


});

app.MapDelete("/teams", async (WorldCupContext context, Guid id) =>
{
    var dbTeam = await context.Teams.FindAsync(id);
    if (dbTeam == null)
        return Results.NotFound();

    context.Teams.Remove(dbTeam);
    await context.SaveChangesAsync();

    return Results.NoContent();

});

app.Run();

public class Team {

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Img { get; set; }

}