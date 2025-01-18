namespace Stopify.Repositories;

public interface IRepository<T>
{
    T? GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    T Update(int id, T entity);
    void Delete(int id);
}