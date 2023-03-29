using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi;



// ******************************************************************blogging database**************************
using var db = new BloggingContext();


// Note: This sample requires the database to be created before running.
Console.WriteLine($"Database path: {db.DbPath}.");

// Create
Console.WriteLine("Inserting a new blog");
db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
db.SaveChanges();


// Read
Console.WriteLine("Querying for a blog");
var blog = db.Blogs
    .OrderBy(b => b.BlogId)
    .First();

// Update
Console.WriteLine("Updating the blog and adding a post");
blog.Url = "https://devblogs.microsoft.com/dotnet";
blog.Posts.Add(
    new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" });
db.SaveChanges();

// Delete
Console.WriteLine("Delete the blog");
//db.Remove(blog);
db.SaveChanges();

//*******************************************************************************************************




//***************************Blogging application endpionts*********************************************
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BloggingContext>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();


//RouteGroupBuilder blogs = app.MapGroup("/blogs");
app.MapGet("/", GetAllBlogs);
app.MapGet("/posts", GetAllPosts);


RouteGroupBuilder todoItems = app.MapGroup("/todoitems");

todoItems.MapGet("/", GetAllTodos);
todoItems.MapGet("/complete", GetCompleteTodos);
todoItems.MapGet("/{id}", GetTodo);
todoItems.MapPost("/", CreateTodo);
todoItems.MapPut("/{id}", UpdateTodo);
todoItems.MapDelete("/{id}", DeleteTodo);

app.Run();

static async Task<IResult> GetAllBlogs(BloggingContext db)
{

    return TypedResults.Ok(await db.Blogs.ToListAsync());
}


//BELOW CAUSES CIRCULAR ERROR
static async Task<IResult> GetAllPosts(BloggingContext db)
{
    // Load all posts from the database including their associated blog
    var posts = await db.Posts.Include(p => p.Blog).ToListAsync();

    // Return the serialized posts as the result of the method
    return TypedResults.Ok(posts);
}


static async Task<IResult> GetAllTodos(BloggingContext db)
{
    return TypedResults.Ok(await db.Todos.ToArrayAsync());
}

static async Task<IResult> GetCompleteTodos(BloggingContext db)
{
    return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).ToListAsync());
}

static async Task<IResult> GetTodo(int id, BloggingContext db)
{
    return await db.Todos.FindAsync(id)
        is Todo todo
            ? TypedResults.Ok(todo)
            : TypedResults.NotFound();
}

static async Task<IResult> CreateTodo(Todo todo, BloggingContext db)
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/todoitems/{todo.Id}", todo);
}

static async Task<IResult> UpdateTodo(int id, Todo inputTodo, BloggingContext db)
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return TypedResults.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeleteTodo(int id, BloggingContext db)
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return TypedResults.Ok(todo);
    }

    return TypedResults.NotFound();
}