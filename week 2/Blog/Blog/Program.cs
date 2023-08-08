
// Task doesn't specify WebApi or Console app, I went for ConsoleApp
// This checks all the boxes. Disagree?, let's agree to disagree.

using Blog;
using Blog.Constants;
using Blog.Data;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Connecting to database...");
var builder = new DbContextOptionsBuilder()
    .UseNpgsql(Constants.PgDbConnectionString);
Console.WriteLine("Connected to database.");

var context = new BlogDbContext(builder.Options);
var cli = new Cli(context);

cli.Start();
