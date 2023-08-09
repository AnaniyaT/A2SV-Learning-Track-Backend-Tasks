namespace UnitTests;

public class PostComparer : IEqualityComparer<Post>
{
    public bool Equals(Post? x, Post? y)
    {
        if (x == null || y == null)
        {
            return false;
        }

        return x.Id == y.Id
               && x.Title == y.Title
               && x.Content == y.Content;
    }

    public int GetHashCode(Post obj)
    {
        return obj.Id.GetHashCode();
    }
}