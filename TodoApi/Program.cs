using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar Base de Datos (SQLite)
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));

var app = builder.Build();

// 2. Definir los Endpoints (Rutas)
// Obtener todos
app.MapGet("/todoitems", async (TodoDb db) =>
    await db.Todos.ToListAsync());

// Crear uno nuevo
app.MapPost("/todoitems", async (Todo item, TodoDb db) =>
{
    db.Todos.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{item.Id}", item);
});

app.Run();

// 3. Clases de soporte
class Todo {
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}

class TodoDb : DbContext {
    public TodoDb(DbContextOptions<TodoDb> options) : base(options) { }
    public DbSet<Todo> Todos => Set<Todo>();
}