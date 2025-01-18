namespace Stopify.Repositories;

public interface IRepository<T>
{
    T? GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    T Update(T entity);
    void Delete(int id);
}