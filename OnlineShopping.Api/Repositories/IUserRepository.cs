using System.Collections.Generic;
using ProjectGitFlow.Models;

namespace ProjectGitFlow.Repositories;

public interface IUserRepository

{

    User? GetById(int id);

    User? GetByEmail(string email);

    IEnumerable<User> GetAll();

    bool EmailExists(string email);

    User Add(User user);

}
