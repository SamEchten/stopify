using Microsoft.EntityFrameworkCore;
using EntityClass = Stopify.Entity.Entity;

namespace Stopify.Repository
{
    public class EntityRepository : IRepository<EntityClass>
    {
        private readonly DbContext _context;
        private readonly DbSet<EntityClass> _dbSet;

        public EntityRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<EntityClass>();
        }
        
        public EntityClass? GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<EntityClass> GetAll()
        {
            return _dbSet.ToList();
        }

        public void Add(EntityClass entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(EntityClass entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(EntityClass entity)
        {
            _dbSet.Remove(entity);
        }
        
    }
}