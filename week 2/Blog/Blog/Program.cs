
// Task doesn't specify WebApi or Console app, I went for ConsoleApp
// This checks all the boxes. Disagree?, let's agree to disagree.

using Blog;
using Blog.Constants;
using Blog.Data;
using Blog.Models;
using Blog.Services;
using Microsoft.EntityFrameworkCore;

var builder = new DbContextOptionsBuilder()
    .UseNpgsql(Constants.PgDbConnectionString);

var context = new BlogDbContext(builder.Options);
var postManager = new PostManager(context);
var commentManager = new CommentManager(context);

Console.WriteLine("Hello, World!");

var display = new BlogDisplay(context);
display.DisplayPosts();
Console.WriteLine("Detailed View");
display.DisplayDetails(6);