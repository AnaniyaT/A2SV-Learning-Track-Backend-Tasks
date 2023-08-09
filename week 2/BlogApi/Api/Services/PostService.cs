using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class PostService
{
    private readonly BlogDbContext _context;

    public PostService(BlogDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Post> GetAll()
    {
        return _context.Posts.AsNoTracking()
            .ToList();
    }

    public Post? GetById(int id)
    {
        return _context.Posts.Find(id);
    }

    public Post Create(Post post)
    {
        _context.Posts.Add(post);
        _context.SaveChanges();

        return post;
    }

    public Post Update(int id, string? title=null, string? content=null)
    {
        Post? post = _context.Posts.Find(id);

        if (post is null)
        {
            throw new InvalidOperationException("Post does not exist");
        }

        post.Title = title ?? post.Title;
        post.Content = content ?? post.Content;
        _context.SaveChanges();

        return post;
    }

    public void Delete(int id)
    {
        Post? post = _context.Posts.Find(id);
        if (post is null)
            throw new InvalidOperationException("Post does not exist");
        
        _context.Posts.Remove(post);
        _context.SaveChanges();
    }
}