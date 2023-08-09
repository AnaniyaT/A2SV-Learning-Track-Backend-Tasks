
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Xunit;

namespace UnitTests;

public class CommentsControllerTests
{
    private readonly List<Comment> _comments = new List<Comment>
    {
        new Comment { Id = 1, PostId = 1, Text = "Comment 1" },
        new Comment { Id = 2, PostId = 2, Text = "Comment 2" },
        new Comment { Id = 3, PostId = 2, Text = "Comment 3" }
    };

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
    public void Get_ReturnsAllComments()
    {
        // Arrange
        var options = GetContextOptions();
        using var context = new BlogDbContext(options);
        var controller = new CommentsController(context);

        var comments = _comments;
        context.Comments.AddRange(comments);
        context.SaveChanges();

        // Act
        var result = controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualComments = Assert.IsAssignableFrom<IEnumerable<Comment>>(okResult.Value);
        Assert.Equal(comments, actualComments, new CommentComparer());
    }

    [Fact]
    public void Get_WithValidId_ReturnsComment()
    {
        // Arrange
        var options = GetContextOptions();
        using var context = new BlogDbContext(options);
        var controller = new CommentsController(context);

        var comment = new Comment { Id = 1, Text = "Test Comment" };
        context.Comments.Add(comment);
        context.SaveChanges();

        // Act
        var result = controller.Get(comment.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualComment = Assert.IsType<Comment>(okResult.Value);
        Assert.Equal(comment, actualComment, new CommentComparer());
    }

    [Fact]
    public void Get_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var options = GetContextOptions();
        using var context = new BlogDbContext(options);
        var controller = new CommentsController(context);

        // Act
        var result = controller.Get(0);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void GetByPostId_ReturnsCommentsWithMatchingPostId()
    {
        // Arrange
        var options = GetContextOptions();
        using var context = new BlogDbContext(options);
        var controller = new CommentsController(context);
        var comments = _comments;
        context.AddRange(comments);
        context.SaveChanges();
        
        // Act
        var result = controller.GetByPostId(2);
        
        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualComment = Assert.IsAssignableFrom<IEnumerable<Comment>>(okResult.Value);
        Assert.Equal(new List<Comment> { comments[1], comments[2] }, actualComment, new CommentComparer());
    }

    [Fact]
    public void Create_CreatesAndReturnsNewlyCreatedComment()
    {
        // Arrange
        var options = GetContextOptions();
        using var context = new BlogDbContext(options);
        var controller = new CommentsController(context);
        var comments = _comments;
        var posts = _posts;
        var content = new CommentContent() { PostId = 1, Text = "Test Comment 1" };
        context.AddRange(posts);
        context.AddRange(comments);
        context.SaveChanges();
        
        // Act
        var result = controller.Create(content);
        
        //Assert
        var okResult = Assert.IsType<CreatedAtActionResult>(result);
        var actualComment = Assert.IsType<Comment>(okResult.Value);
        var expected = new Comment() { Id = 4, PostId = 1, Text = "Test Comment 1" };
        Assert.Equal(expected, actualComment, new CommentComparer());
        Assert.Equal(expected, context.Comments.Find(4), new CommentComparer());
    }
    
    [Fact]
    public void Create_WithInvalidPostId_ReturnsBadRequest()
    {
        // Arrange
        var options = GetContextOptions();
        using var context = new BlogDbContext(options);
        var controller = new CommentsController(context);
        var comments = _comments;
        var posts = _posts;
        var content = new CommentContent() { PostId = 4, Text = "Test Comment 1" };
        context.AddRange(posts);
        context.AddRange(comments);
        context.SaveChanges();
        
        // Act
        var result = controller.Create(content);
        
        //Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void Patch_UpdatesAndReturnsCommentWithMatchingId()
    {
        // Arrange
        var options = GetContextOptions();
        using var context = new BlogDbContext(options);
        var controller = new CommentsController(context);

        var comments = _comments;
        context.Comments.AddRange(comments);
        context.SaveChanges();
        
        // Act
        var result = controller.Patch(2, new CommentContent() { Text = "Comment 2 Edited" });
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualComment = Assert.IsType<Comment>(okResult.Value);
        Assert.Equal(new Comment() { Id = 2, PostId = 2, Text = "Comment 2 Edited"}, actualComment, new CommentComparer());
        Assert.Equal("Comment 2 Edited", context.Comments.Find(2).Text);
    }

    [Fact]
    public void Patch_WithInvalidId_ReturnsBadRequest()
    {
        // Arrange
        var options = GetContextOptions();
        using var context = new BlogDbContext(options);
        var controller = new CommentsController(context);

        var comment = new Comment { Id = 1, Text = "Test Comment" };
        context.Comments.Add(comment);
        context.SaveChanges();
        
        // Act
        var result = controller.Patch(2, new CommentContent() { Text = "Test Comment edited" });
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void Delete_DeletesAndReturnsNoContent()
    {
        // Arrange
        var options = GetContextOptions();
        using var context = new BlogDbContext(options);
        var controller = new CommentsController(context);

        var comments = _comments;
        context.Comments.AddRange(comments);
        context.SaveChanges();
        
        // Act
        var result = controller.Delete(1);
        
        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Null(context.Comments.Find(1));
    }

    [Fact]
    public void Delete_WithInvalidId_ReturnsBadRequest()
    {
        // Arrange
        var options = GetContextOptions();
        using var context = new BlogDbContext(options);
        var controller = new CommentsController(context);

        var comments = _comments;
        context.Comments.AddRange(comments);
        context.SaveChanges();
        
        // Act
        var result = controller.Delete(4);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}
