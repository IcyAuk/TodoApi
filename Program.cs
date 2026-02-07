using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
//add dbContext and enable database exceptions
//Dependency Injection provides access to the dbContext
builder.Services.AddDbContext<TodoDb>
    (opt => opt.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=TodoDb;Integrated Security=true;"));
    //(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
   config.DocumentName = "TodoAPI";
   config.Title = "TodoAPI v1";
   config.Version = "v1"; 
});

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "TodoAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

var todoItems = app.MapGroup("/todoitems");
todoItems.MapGet("/",GetAllTodos);
todoItems.MapGet("/complete",GetCompleteTodos);
todoItems.MapGet("/{id}",GetTodo);
todoItems.MapPost("/",CreateTodo);
todoItems.MapPut("/{id}",UpdateTodo);
todoItems.MapDelete("/{id}",DeleteTodo);

/*
app: web app object
.mapget(string endpoint, delegate func): register HTTP GET endpoint.
    delegate is a function/method stored in a var.

*/
static async Task<IResult> GetAllTodos(TodoDb db)
{
    return TypedResults.Ok(
        await db.Todos
        //foreach todo in db.Todos
            .Select(todo => new TodoItemDTO(todo))
            .ToArrayAsync()
    );
}

static async Task<IResult> GetCompleteTodos(TodoDb db)
{
    return TypedResults.Ok(
        await db.Todos
            .Where(t => t.IsComplete)
            .Select(x => new TodoItemDTO(x))
            .ToListAsync()
    );
}

static async Task<IResult> GetTodo(int id, TodoDb db)
{
    return await db.Todos.FindAsync(id)
        is Todo todo
        ? TypedResults.Ok(todo)
        : TypedResults.NotFound();
}

//DTO defines what the clien can send. Layer of protection from the Model
//In this case Id property cannot be sent by the client.
static async Task<IResult> CreateTodo(TodoItemDTO todoItemDTO, TodoDb db)
{
    var todoItem = new Todo
    {
        Name = todoItemDTO.Name,
        IsComplete = todoItemDTO.IsComplete
    };

    db.Todos.Add(todoItem);
    await db.SaveChangesAsync();
    
    return TypedResults.Created($"/todoitems/{todoItem.Id}",todoItemDTO);
}

static async Task<IResult> UpdateTodo(int id, TodoItemDTO todoItemDTO, TodoDb db)
{
    var todo = await db.Todos.FindAsync(id);
    if(todo is null) return TypedResults.NotFound();

    todo.Name = todoItemDTO.Name;
    todo.IsComplete = todoItemDTO.IsComplete;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeleteTodo(int id, TodoDb db)
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
    return TypedResults.NotFound();
}

app.Run();
