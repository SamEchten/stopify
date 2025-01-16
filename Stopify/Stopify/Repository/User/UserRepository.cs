using Microsoft.EntityFrameworkCore;
using UserEntity = Stopify.Entity.User.User;

namespace Stopify.Repository.User
{
    public class UserRepository : EntityRepository<UserEntity>
    {
        public UserRepository(ApplicationDbContext context): base(context)
        {
        }
        
        public UserEntity? GetById(int id)
        {
            return DbSet.Find(id);
        }

        public IEnumerable<UserEntity> GetAll()
        {
            return DbSet.ToList();
        }

        public void Add(UserEntity entity)
        {
            DbSet.Add(entity);
        }

        public void Update(UserEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(UserEntity entity)
        {
            DbSet.Remove(entity);
        }
        
    }
}