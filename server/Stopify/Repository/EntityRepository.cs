using Microsoft.EntityFrameworkCore;

namespace Stopify.Repository;
public abstract class EntityRepository<TEntity>(ApplicationDbContext context) : IRepository<TEntity> where TEntity : Entity.Entity
{
    protected readonly ApplicationDbContext Context = context;
    protected readonly DbSet<TEntity> DbSet = context.Set<TEntity>();

    public TEntity? GetById(int id)
    {
        return DbSet.Find(id);
    }

    public IEnumerable<TEntity> GetAll()
    {
        return DbSet.ToList();
    }

    public void Add(TEntity entity)
    {
        DbSet.Add(entity);

        Context.SaveChanges();
    }

    public TEntity Update(int id, TEntity entity)
    {
        var existingEntity = DbSet.Find(id)!;

        foreach (var property in Context.Entry(existingEntity).Properties)
        {
            if (property.Metadata.IsPrimaryKey())
            {
                continue;
            }

            var newValue = Context.Entry(entity).Property(property.Metadata.Name).CurrentValue;
            property.CurrentValue = newValue;
        }

        Context.SaveChanges();

        return existingEntity;
    }

    public void Delete(int id)
    {
        var entity = DbSet.Find(id)!;

        DbSet.Remove(entity);

        Context.SaveChanges();
    }
}