using Blog.Data;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services;

public class CommentManager
{
    private readonly BlogDbContext _context;

    public CommentManager(BlogDbContext context)
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
            return;

        _context.Comments.Remove(comment);
        _context.SaveChanges();
    }
}