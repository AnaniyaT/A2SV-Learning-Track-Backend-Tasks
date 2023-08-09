using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UnitTests;

public class PostControllerTest
{
    private readonly List<Post> _posts = new List<Post>
    {
        new Post { Id = 1, Title = "Title 1", Content = "Content 1" },
        new Post { Id = 2, Title = "Title 2", Content = "Content 2" },
        new Post { Id = 3, Title = "Title 3", Content = "Content 3" }
    };
    
    private DbContextOptions<BlogDbContext> GetContextOptions()
    {
        return new DbContextOptionsBuilder<BlogDbContext>()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
            .Options;
    }
    
    [Fact]
    public void Get_ReturnsAllPosts()
    {
        // Arrange
        var options = GetContextOptions();
        using var context = new BlogDbContext(options);
        var controller = new PostsController(context);

        var posts = _posts;
        context.Posts.AddRange(posts);
        context.SaveChanges();

        // Act
        var result = controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualPosts = Assert.IsAssignableFrom<IEnumerable<Post>>(okResult.Value);
        Assert.Equal(posts, actualPosts, new PostComparer());
    }

    [Fact]
    public void Get_WithValidId_ReturnsPostWithMatchingId()
    {
        // Arrange
        var options = GetContextOptions();
        using var context = new BlogDbContext(options);
        var controller = new PostsController(context);

        var posts = _posts;
        context.Posts.AddRange(posts);
        context.SaveChanges();
        
        // Act
        var result = controller.Get(1);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualPost = Assert.IsType<Post>(okResult.Value);
        Assert.Equal(posts[0], actualPost, new PostComparer());
    }
    
    [Fact]
    public void Get_WithInValidId_ReturnsNotFound()
    {
        // Arrange
        var options = GetContextOptions();
        using var context = new BlogDbContext(options);
        var controller = new PostsController(context);

        var posts = _posts;
        context.Posts.AddRange(posts);
        context.SaveChanges();
        
        // Act
        var result = controller.Get(4);
        
        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void Post_CreatesAndReturnsNewlyCreatedPost()
    {
        // Arrange
        var options = GetContextOptions();
        using var context = new BlogDbContext(options);
        var controller = new PostsController(context);

        var posts = _posts;
        context.Posts.AddRange(posts);
        context.SaveChanges();
        
        // Act
        var content = new PostContents() { Title = "New title", Content = "New content" };
        var result = controller.Post(content);
        
        // Assert
        var expected = new Post() { Id = 4, Title = "New title", Content = "New content" };
        var okResult = Assert.IsType<CreatedAtActionResult>(result);
        var actualPost = Assert.IsType<Post>(okResult.Value);
        Assert.Equal(expected, actualPost, new PostComparer());
        Assert.Equal(expected, context.Posts.Find(4), new PostComparer());
    }

    [Fact]
    public void Patch_UpdatesAndReturnsPostWithMatchingId()
    {
        // Arrange
        var options = GetContextOptions();
        using var context = new BlogDbContext(options);
        var controller = new PostsController(context);

        var posts = _posts;
        context.Posts.AddRange(posts);
        context.SaveChanges();
        
        // Act
        var content = new PostContents() { Title = "Edited title", Content = "Edited content" };
        var result = controller.Patch(1, content);
        
        // Assert
        var expected = new Post() { Id = 1, Title = "Edited title", Content = "Edited content" };
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualPost = Assert.IsType<Post>(okResult.Value);
        Assert.Equal(expected, actualPost, new PostComparer());
        Assert.Equal(expected, context.Posts.Find(1), new PostComparer());
    }

    [Fact]
    public void Patch_WithInvalidId_ReturnsBadRequest()
    {
        // Arrange
        var options = GetContextOptions();
        using var context = new BlogDbContext(options);
        var controller = new PostsController(context);

        var posts = _posts;
        context.Posts.AddRange(posts);
        context.SaveChanges();
        
        // Act
        var content = new PostContents() { Title = "Edited title", Content = "Edited content" };
        var result = controller.Patch(4, content);
        
        //Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void Delete_WithValidId_DeletesPostAndReturnsNoContent()
    {
        // Arrange
        var options = GetContextOptions();
        using var context = new BlogDbContext(options);
        var controller = new PostsController(context);

        var posts = _posts;
        context.Posts.AddRange(posts);
        context.SaveChanges();
        
        // Act
        var result = controller.Delete(1);
        
        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Null(context.Posts.Find(1));
    }

    [Fact]
    public void Delete_WithInvalidId_ReturnsBadRequest()
    {
        // Arrange
        var options = GetContextOptions();
        using var context = new BlogDbContext(options);
        var controller = new PostsController(context);

        var posts = _posts;
        context.Posts.AddRange(posts);
        context.SaveChanges();
        
        // Act
        var result = controller.Delete(4);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}