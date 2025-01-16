using Microsoft.EntityFrameworkCore;
using UserEntity = Stopify.Entity.User.User;

namespace Stopify.Repository.User
{
    public class UserRepository : IRepository<UserEntity>
    {
        private readonly DbContext _context;
        private readonly DbSet<UserEntity> _dbSet;

        public UserRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<UserEntity>();
        }
        
        public UserEntity? GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<UserEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        public void Add(UserEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(UserEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(UserEntity entity)
        {
            _dbSet.Remove(entity);
        }
        
    }
}