using Microsoft.EntityFrameworkCore;
using ToDo.Context;
using ToDo.Models;

var builder = WebApplication.CreateBuilder(args);

//add DI - Services
builder.Services.AddDbContext<ToDoDB>(opt => opt.UseInMemoryDatabase("ToDoList"));
var app = builder.Build();

//add configurations
app.MapGet("/todoitems", async (ToDoDB db) => await db.Todos.ToListAsync());
app.MapGet("/todoitems/{id}", async (int id, ToDoDB db) => await db.Todos.FirstOrDefaultAsync(i => i.Id == id));
app.MapPost("/todoitems", async (ToDoItem item, ToDoDB db) =>
{
    await db.Todos.AddAsync(item);
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{item.Id}", item);
});

app.MapPut("/todoitems/{id}", async (int id, ToDoItem item, ToDoDB db) =>
{
    var todo = await db.Todos.FirstOrDefaultAsync(i => i.Id == id);
    if (todo == null) return Results.NotFound();
    todo.Name = item.Name;
    todo.IsCompleted = item.IsCompleted;
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{id}", todo);
});

app.MapDelete("/todoitems/{id}", async (int id, ToDoDB db) =>
{
    var todo = await db.Todos.FirstOrDefaultAsync(i => i.Id == id);
    if (todo == null) return Results.NotFound();
    db.Remove(todo);
    await db.SaveChangesAsync();
    return Results.NoContent();
});
app.Run();