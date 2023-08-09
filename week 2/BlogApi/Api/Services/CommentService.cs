using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class CommentService
{
    private readonly BlogDbContext _context;

    public CommentService(BlogDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Comment> GetAll()
    {
        return _context.Comments.AsNoTracking()
            .ToList();
    }

    public Comment? GetById(int id)
    {
        return _context.Comments.Find(id);
    }

    public IEnumerable<Comment> GetByPostId(int id)
    {
        return _context.Comments
        .Where(c => c.PostId == id)
        .ToList();
    }

    public Comment Create(Comment comment)
    {
        Post? post = _context.Posts.Find(comment.PostId);
        if (post is null)
        {
            throw new InvalidOperationException("Post does not exist");
        }
        
        _context.Comments.Add(comment);
        _context.SaveChanges();

        return comment;
    }

    public Comment Update(int id, string text)
    {
        Comment? comment = _context.Comments.Find(id);

        if (comment is null)
        {
            throw new InvalidOperationException("Comment does not exist");
        }

        comment.Text = text ?? comment.Text;
        _context.SaveChanges();

        return comment;
    }

    public void Delete(int id)
    {
        Comment? comment = _context.Comments.Find(id);
        if (comment is null)
            throw new InvalidOperationException("Comment does not exist");

        _context.Comments.Remove(comment);
        _context.SaveChanges();
    }
}