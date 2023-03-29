namespace TodoApi;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Todo> Todos { get; set; }

    public string DbPath { get; }

    public BloggingContext() 
    {
        var folderPath = Path.GetDirectoryName(Environment.CurrentDirectory);
        DbPath = Path.Combine(folderPath, "blogging.db");
    }


    // The following configures EF to create a Sqlite database file in the
    // same folder as the executable.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }

    public List<Post> Posts { get; } = new();
}

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}

public class Todo
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
    
}