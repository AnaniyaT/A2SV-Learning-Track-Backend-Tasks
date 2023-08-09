namespace Api.Models;

public class Comment : BlogItem
{
    public int PostId { get; set; }
    public string Text { get; set; } = "";

    public virtual Post Post { get; set; }
}