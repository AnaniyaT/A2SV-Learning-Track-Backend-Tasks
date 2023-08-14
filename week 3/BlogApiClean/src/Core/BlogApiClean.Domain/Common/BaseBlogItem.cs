namespace BlogApiClean.Domain.Common;

public abstract class BaseBlogItem
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
}