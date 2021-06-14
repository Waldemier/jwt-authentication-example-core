using jwt_server.Models;

namespace jwt_server.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User Create(User user);
        User GetByEmail(string email);
        User GetById(int Id);
    }
}