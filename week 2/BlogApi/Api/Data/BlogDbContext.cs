using Microsoft.EntityFrameworkCore;
using Api.Models;
namespace Api.Data;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions options) : base(options) {}
    
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasOne(p => p.Post)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.PostId)
                .HasConstraintName("FK_Post_Id");
        });

        modelBuilder.Entity<Post>().Property(p => p.CreatedAt)
            .HasDefaultValueSql("now()");
        
        modelBuilder.Entity<Comment>().Property(c => c.CreatedAt)
            .HasDefaultValueSql("now()");
    }
}