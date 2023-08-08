using Blog.Data;
using Blog.Services;

namespace Blog;

public class BlogDisplay
{
    private PostManager _postManager;
    private CommentManager _commentManager;

    public BlogDisplay(BlogDbContext context)
    {
        _postManager = new PostManager(context);
        _commentManager = new CommentManager(context);
    }

    public void DisplayPosts()
    {
        Console.WriteLine("    ------------- Posts --------------");
        foreach (var post in _postManager.GetAll())
        {
            Console.WriteLine($"""
            ----------------------------------
            {post.Title}
            {post.Content}
            Post #{post.Id}       {post.CreatedAt}
            ----------------------------------
        """);
        }
    }

    public void DisplayDetails(int id)
    {
        var post = _postManager.GetById(id);

        if (post is null)
        {
            Console.WriteLine("Post Not Found");
            return;
        }

        var comments = _commentManager.GetByPostId(id);
        Console.WriteLine($"""
            {post.Title}
            {post.Content}
            Post #{post.Id}       {post.CreatedAt}
            -------------------------------------
        """);
        int numberOfComms = comments.Count();
        
        Console.WriteLine((numberOfComms == 0 ? "        No Comments" : $"        Comments({numberOfComms})\n") );
        foreach (var comment in comments)
        {
            Console.WriteLine($"""
                {comment.Text}
                #{comment.Id}    {comment.CreatedAt}
                -----------------------------
        """);
        }
    }
}