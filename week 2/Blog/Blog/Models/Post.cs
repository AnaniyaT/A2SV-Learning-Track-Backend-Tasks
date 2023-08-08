namespace Blog.Models;

public class Post
{
    public Post()
    {
        Comments = new HashSet<Comment>();
    }
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    
}