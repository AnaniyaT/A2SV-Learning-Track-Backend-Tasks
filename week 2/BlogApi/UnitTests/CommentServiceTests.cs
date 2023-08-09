using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Xunit.Sdk;

namespace UnitTests;

public class CommentServiceTests
{
    [Fact]
    public void GetAll_ReturnsAllComments()
    {
        // Arrange
        var comments = new List<Comment>
        {
            new Comment { PostId = 1, Text = "Comment 1" },
            new Comment { PostId = 1, Text = "Comment 2" },
            new Comment { PostId = 2, Text = "Comment 3" }
        };
        
        var options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseInMemoryDatabase(databaseName:$"{Guid.NewGuid()}")
            .Options;
        
        using var context = new BlogDbContext(options);
        context.Comments.AddRange(comments);
        context.SaveChanges();
        var commentService = new CommentService(context);
        var comparer = new CommentComparer();

        // Act
        var result = commentService.GetAll();

        // Assert
        Assert.Equal(comments, result, comparer);
    }

    [Fact]
    public void GetById_ReturnsCommentWithMatchingId()
    {
        // Arrange
        var comments = new List<Comment>
        {
            new Comment { Id = 1, PostId = 1, Text = "Comment 1" },
            new Comment { Id = 2, PostId = 1, Text = "Comment 2" },
            new Comment { Id = 3, PostId = 2, Text = "Comment 3" }
        };
        var options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
            .Options;
        using var context = new BlogDbContext(options);
        context.Comments.AddRange(comments);
        context.SaveChanges();
        var commentService = new CommentService(context);
        var comparer = new CommentComparer();

        // Act
        var result = commentService.GetById(2);

        // Assert
        Assert.Equal(comments[1], result);
    }

    [Fact]
    public void GetByPostID_ReturnsCommentsWithMatchingPostID()
    {
        // Arrange
        var comments = new List<Comment>
        {
            new Comment { Id = 1, PostId = 1, Text = "Comment 1" },
            new Comment { Id = 2, PostId = 1, Text = "Comment 2" },
            new Comment { Id = 3, PostId = 2, Text = "Comment 3" }
        };
        var options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
            .Options;
        using var context = new BlogDbContext(options);
        context.Comments.AddRange(comments);
        context.SaveChanges();
        var commentService = new CommentService(context);
        var comparer = new CommentComparer();
        
        var expected = new List<Comment>
        {
            new Comment { Id = 1, PostId = 1, Text = "Comment 1" },
            new Comment { Id = 2, PostId = 1, Text = "Comment 2" },
        };

        // Act
        var result = commentService.GetByPostId(1);

        // Assert
        Assert.Equal(expected, result, comparer);
    }

    [Fact]
    public void Create_CreatesAndReturnsNewlyCreatedComment()
    {
        // Arrange
        var comments = new List<Comment>
        {
            new Comment { Id = 1, PostId = 1, Text = "Comment 1" },
            new Comment { Id = 2, PostId = 1, Text = "Comment 2" },
            new Comment { Id = 3, PostId = 2, Text = "Comment 3" }
        };
        var post = new Post() { Id = 2, Title = "Title", Content = "Content" };
        var options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
            .Options;
        using var context = new BlogDbContext(options);
        context.Comments.AddRange(comments);
        context.Posts.Add(post);
        context.SaveChanges();
        var commentService = new CommentService(context);
        var commentToCreate = new Comment() { Id = 4, PostId = 2, Text = "Comment 4"};
        var comparer = new CommentComparer();

        // Act
        var result = commentService.Create(commentToCreate);

        // Assert
        Assert.Equal(commentToCreate, result, comparer);
        Assert.Equal(commentToCreate, context.Comments.Find(4), comparer);
    }

    [Fact]
    public void Update_UpdatesAndReturnsUpdatedComment()
    {
        // Arrange
        var comments = new List<Comment>
        {
            new Comment { Id = 1, PostId = 1, Text = "Comment 1" },
            new Comment { Id = 2, PostId = 1, Text = "Comment 2" },
            new Comment { Id = 3, PostId = 2, Text = "Comment 3" }
        };
        var options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
            .Options;
        using var context = new BlogDbContext(options);
        context.Comments.AddRange(comments);
        context.SaveChanges();
        var commentService = new CommentService(context);
        var comparer = new CommentComparer();

        var expected = new Comment { Id = 2, PostId = 1, Text = "Comment 2 edited" };

        // Act
        var result = commentService.Update(2, "Comment 2 edited");

        // Assert
        Assert.Equal(expected, result, comparer);
        Assert.Equal(expected, context.Comments.Find(2), comparer);
    }

    [Fact]
    public void Update_ThrowsExceptionWhenCommentDoesNotExist()
    {
        var comments = new List<Comment>
        {
            new Comment { Id = 1, PostId = 1, Text = "Comment 1" },
            new Comment { Id = 2, PostId = 1, Text = "Comment 2" },
            new Comment { Id = 3, PostId = 2, Text = "Comment 3" }
        };
        var options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
            .Options;
        using var context = new BlogDbContext(options);
        context.Comments.AddRange(comments);
        context.SaveChanges();
        var commentService = new CommentService(context);
       
        // Act and Assert
        Assert.Throws<InvalidOperationException>(() => commentService.Update(4, "Comment 4 edited"));
    }

    [Fact]
    public void Delete_DeletesCommentWithMatchingId()
    {
        // Arrange
        var comments = new List<Comment>
        {
            new Comment { Id = 1, PostId = 1, Text = "Comment 1" },
            new Comment { Id = 2, PostId = 1, Text = "Comment 2" },
            new Comment { Id = 3, PostId = 2, Text = "Comment 3" }
        };
        var options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseInMemoryDatabase(databaseName: $"{Guid.NewGuid()}")
            .Options;
        using var context = new BlogDbContext(options);
        context.Comments.AddRange(comments);
        context.SaveChanges();
        var commentService = new CommentService(context);

        // Act
        commentService.Delete(2);

        // Assert
        Assert.Null(context.Comments.Find(2));
    }
}