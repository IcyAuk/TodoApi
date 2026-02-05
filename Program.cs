using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>
    (opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/todoitems",async(TodoDb db) =>
    await db.Todos.ToListAsync());

/*
app: web app object
.mapget(string endpoint, delegate func): register HTTP GET endpoint.
    delegate is a function/method stored in a var.

*/
app.MapGet("todoitems/complete",async(TodoDb db) =>
    await db.Todos.Where(t => t.IsComplete).ToListAsync());

/*
Second param is very cramped.
Async Lamba expression, pattern matching, terniary operator
Pattern Matching is Todo todo checks if .FindAsync() returns non null Todo
if true then assign 
*/
app.MapGet("todoitems/{id}",
    async(int id, TodoDb db) =>
    await db.Todos.FindAsync(id)
    is Todo todo
    ? Results.Ok(todo)
    : Results.NotFound());

