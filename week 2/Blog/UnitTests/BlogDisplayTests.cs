using Blog.Data;
using Blog.Services;
using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Blog.Constants;

namespace Blog.Tests
{
    public class BlogDisplayTests : IDisposable
    {
        private readonly BlogDbContext _context;
        private readonly PostManager _postManager;
        private readonly CommentManager _commentManager;
        private readonly StringWriter _output;

        public BlogDisplayTests()
        {
            var _builder = new DbContextOptionsBuilder()
                .UseNpgsql(Constants.Constants.PgDbConnectionString);
            _context = new BlogDbContext(_builder.Options);
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
            var post = new Post { Title = "Test Post", Content = "Test Content" };
            _postManager.Create(post);

            // Act
            blogDisplay.DisplayDetails(post.Id);

            // Assert
            Assert.Contains(post.Title, _output.ToString());
            Assert.Contains(post.Content, _output.ToString());
        }
    }
}