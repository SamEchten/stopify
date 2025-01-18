﻿using System.Runtime.InteropServices.JavaScript;
using Stopify.Entities.Users;

namespace Stopify.Services.Users;

public class UserFactory(HashingService hashingService) : IFactory
{
    public User Build(string username, string email, string password)
    {
        return new User
        {
            Username = username,
            Email = email,
            Password = hashingService.HashPassword(password),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
    }
}