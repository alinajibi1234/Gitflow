using System;
using System.Collections.Generic;
using System.Linq;
using ProjectGitFlow.Models;

namespace ProjectGitFlow.Repositories;

public class UserRepository : IUserRepository

{

    private readonly List<User> _users = new();

    private readonly object _lock = new();

    private int _nextId = 1;

    public User? GetById(int id)

    {

        lock (_lock) { return _users.FirstOrDefault(u => u.Id == id); }

    }

    public User? GetByEmail(string email)

    {

        lock (_lock)

        {

            return _users.FirstOrDefault(u =>

                string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));

        }

    }

    public IEnumerable<User> GetAll()

    {

        lock (_lock) { return _users.ToList(); }

    }

    public bool EmailExists(string email)

    {

        lock (_lock)

        {

            return _users.Any(u =>

                string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));

        }

    }

    public User Add(User user)

    {

        lock (_lock)

        {

            user.Id = _nextId++;

            _users.Add(user);

            return user;

        }

    }

}
