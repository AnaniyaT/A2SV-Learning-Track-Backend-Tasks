using BlogApiClean.Domain.Common;

namespace BlogApiClean.Domain;

public class Post : BaseBlogItem
{
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
}