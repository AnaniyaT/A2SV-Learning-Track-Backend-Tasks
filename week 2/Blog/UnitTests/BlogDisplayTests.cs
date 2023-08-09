using Blog.Data;
using Blog.Services;
using System;
using System.IO;
using Blog;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Blog.Constants;

namespace UnitTests;

public class BlogDisplayTests : IDisposable
{
    private readonly BlogDbContext _context;
    private readonly PostManager _postManager;
    private readonly CommentManager _commentManager;
    private readonly StringWriter _output;

    public BlogDisplayTests()
    {
        var builder = new DbContextOptionsBuilder()
            .UseInMemoryDatabase(databaseName:$"{Guid.NewGuid()}");
        _context = new BlogDbContext(builder.Options);
        _postManager = new PostManager(_context);
        _commentManager = new CommentManager(_context);
        _output = new StringWriter();
        Console.SetOut(_output);
    }

    public void Dispose()
    {
        _context.Dispose();
        _output.Dispose();
    }

    [Fact]
    public void DisplayPosts_ShouldWritePostsToConsole()
    {
        // Arrange
        var post = new Post { Title = "Test Post", Content = "Test Content" };
        _postManager.Create(post);
        var blogDisplay = new BlogDisplay(_context);

        // Act
        blogDisplay.DisplayPosts();

        // Assert
        Assert.Contains("Posts", _output.ToString());
        Assert.Contains("Post #", _output.ToString());
    }

    [Fact]
    public void DisplayDetails_WithInvalidId_ShouldWritePostNotFoundToConsole()
    {
        // Arrange
        var blogDisplay = new BlogDisplay(_context);

        // Act
        blogDisplay.DisplayDetails(0);

        // Assert
        Assert.Contains("Post Not Found", _output.ToString());
    }

    [Fact]
    public void DisplayDetails_WithValidId_ShouldWritePostDetailsToConsole()
    {
        // Arrange
        var blogDisplay = new BlogDisplay(_context);
        var post = new Post { Title = "Test Post 1", Content = "Test Content 1" };
        _postManager.Create(post);

        // Act
        blogDisplay.DisplayDetails(post.Id);

        // Assert
        Assert.Contains(post.Title, _output.ToString());
        Assert.Contains(post.Content, _output.ToString());
    }
}
