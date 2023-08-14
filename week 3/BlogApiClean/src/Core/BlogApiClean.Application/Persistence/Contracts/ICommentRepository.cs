using BlogApiClean.Domain;

namespace BlogApiClean.Application.Persistence.Contracts;

public interface ICommentRepository : IGenericRepository<Comment>
{
    public Task<IReadOnlyList<Comment>> GetByPostId(int id);
}