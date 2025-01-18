﻿using Stopify.Entities.Users;
using Stopify.Entities.Users.DTO;

namespace Stopify.Repositories.Users;

public class UserRepository(ApplicationDbContext context) : EntityRepository<User>(context)
{
    public new IEnumerable<GetAllUsersDTO> GetAll()
    {
        return DbSet.Select(user => new GetAllUsersDTO(user.Id, user.Username)).ToList();
    }

    public User? GetByEmail(string email)
    {
        return DbSet.FirstOrDefault(user => user.Email == email);
    }

    public User? GetByUsername(string username)
    {
        return DbSet.FirstOrDefault(user => user.Username == username);
    }
}