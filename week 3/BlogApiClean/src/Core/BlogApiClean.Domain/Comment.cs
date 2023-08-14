using BlogApiClean.Domain.Common;

namespace BlogApiClean.Domain;

public class Comment : BaseBlogItem
{
    public int PostId { get; set; }
    public string Text { get; set; } = "";
}