using Microsoft.EntityFrameworkCore;

namespace UnitTests;

public class PostManagerTests
{
    [Fact]
    public void GetAll_ReturnsAllPosts()
    {
        // Arrange
        var posts = new List<Post>
        {
            new Post { Id = 1, Title = "Post 1", Content = "Content 1" },
            new Post { Id = 2, Title = "Post 2", Content = "Content 2" },
            new Post { Id = 3, Title = "Post 3", Content = "Content 3" }
        };
        var options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
            .Options;
        using var context = new BlogDbContext(options);
        context.Posts.AddRange(posts);
        context.SaveChanges();
        var postManager = new PostManager(context);
        var comparer = new PostComparer();

        // Act
        var result = postManager.GetAll();

        // Assert
        Assert.Equal(posts, result, comparer);
        context.Dispose();
    }

    [Fact]
    public void GetById_ReturnsPostWithMatchingId()
    {
        // Arrange
        var posts = new List<Post>
        {
            new Post { Id = 1, Title = "Post 1", Content = "Content 1" },
            new Post { Id = 2, Title = "Post 2", Content = "Content 2" },
            new Post { Id = 3, Title = "Post 3", Content = "Content 3" }
        };

        var options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
            .Options;

        using var context = new BlogDbContext(options);
        context.Posts.AddRange(posts);
        context.SaveChanges();
        var postManager = new PostManager(context);
        var comparer = new PostComparer();
        
        // Act
        var result = postManager.GetById(2);
        
        // Assert
        Assert.Equal(posts[1], result, comparer);
    }

    [Fact]
    public void Create_CreatesAndReturnsNewlyCreatedPost()
    {
        // Arrange
        var posts = new List<Post>
        {
            new Post { Id = 1, Title = "Post 1", Content = "Content 1" },
            new Post { Id = 2, Title = "Post 2", Content = "Content 2" },
            new Post { Id = 3, Title = "Post 3", Content = "Content 3" }
        };

        var options = new DbContextOptionsBuilder()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
            .Options;
        using var context = new BlogDbContext(options);
        context.AddRange(posts);
        context.SaveChanges();
        var postManager = new PostManager(context);
        var postToCreate = new Post() { Id = 4, Title = "Post 4", Content = "Content 4" };
        var comparer = new PostComparer();
        
        // Act
        var result = postManager.Create(postToCreate);
        var getResult = context.Posts.Find(postToCreate.Id);
        
        // Assert
        Assert.Equal(postToCreate, result, comparer);
        Assert.Equal(postToCreate, getResult, comparer);
    }

    [Fact]
    public void Update_UpdatesAndReturnsUpdatedPost()
    {
        // Arrange
        var posts = new List<Post>
        {
            new Post { Id = 1, Title = "Post 1", Content = "Content 1" },
            new Post { Id = 2, Title = "Post 2", Content = "Content 2" },
            new Post { Id = 3, Title = "Post 3", Content = "Content 3" }
        };

        var options = new DbContextOptionsBuilder()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
            .Options;
        using var context = new BlogDbContext(options);
        context.AddRange(posts);
        context.SaveChanges();
        var postManager = new PostManager(context);
        var comparer = new PostComparer();

        Post expected1 = new Post { Id = 1, Title = "Post 1 edited", Content = "Content 1" };
        Post expected2 = new Post { Id = 2, Title = "Post 2", Content = "Content 2 edited" };
        Post expected3 = new Post { Id = 3, Title = "Post 3 edited", Content = "Content 3 edited" };
        
        // Act
        var result1 = postManager.Update(1, title: "Post 1 edited");
        var getResult1 = context.Posts.Find(1);

        var result2 = postManager.Update(2, content: "Content 2 edited");
        var getResult2 = context.Posts.Find(2);

        var result3 = postManager.Update(3, "Post 3 edited", "Content 3 edited");
        var getResult3 = context.Posts.Find(3);
        
        // Assert
        Assert.Equal(expected1, result1, comparer);
        Assert.Equal(expected1, getResult1, comparer);
        
        Assert.Equal(expected2, result2, comparer);
        Assert.Equal(expected2, getResult2, comparer);
        
        Assert.Equal(expected3, result3, comparer);
        Assert.Equal(expected3, getResult3, comparer);
    }

    [Fact]
    public void Update_ThrowsExceptionWhenPostDoesNotExist()
    {
        // Arrange
        var posts = new List<Post>
        {
            new Post { Id = 1, Title = "Post 1", Content = "Content 1" },
            new Post { Id = 2, Title = "Post 2", Content = "Content 2" },
            new Post { Id = 3, Title = "Post 3", Content = "Content 3" }
        };

        var options = new DbContextOptionsBuilder()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
            .Options;
        using var context = new BlogDbContext(options);
        context.AddRange(posts);
        context.SaveChanges();
        var postManager = new PostManager(context);
        
        // Act and Assert
        Assert.Throws<InvalidOperationException>(() => postManager.Update(4, "Post 4 edited"));
    }

    [Fact]
    public void Delete_DeletesPostWithMatchingId()
    {
        // Arrange
        var posts = new List<Post>
        {
            new Post { Id = 1, Title = "Post 1", Content = "Content 1" },
            new Post { Id = 2, Title = "Post 2", Content = "Content 2" },
            new Post { Id = 3, Title = "Post 3", Content = "Content 3" }
        };

        var options = new DbContextOptionsBuilder()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
            .Options;
        using var context = new BlogDbContext(options);
        context.AddRange(posts);
        context.SaveChanges();
        var postManager = new PostManager(context);
        
        // Act
        postManager.Delete(2);
        
        // Assert
        Assert.Null(context.Posts.Find(2));
    }
}