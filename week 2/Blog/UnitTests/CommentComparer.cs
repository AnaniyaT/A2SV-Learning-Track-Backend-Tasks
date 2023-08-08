namespace UnitTests;

public class CommentComparer : IEqualityComparer<Comment>
{
    public bool Equals(Comment? x, Comment? y)
    {
        if (x == null || y == null)
        {
            return false;
        }

        return x.Id == y.Id
               && x.Text == y.Text;
    }

    public int GetHashCode(Comment obj)
    {
        return obj.Id.GetHashCode();
    }
}