namespace BlogApiClean.Application.Persistence.Contracts;

public interface IGenericRepository<T>  where T : class
{
    Task<T> Get(int id);
    Task<IReadOnlyList<T>> GetAll();
    Task<T> Add(T item);
    Task<T> Update(T item);
    Task<T> Delete(T item);
}