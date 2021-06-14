using System.Linq;
using jwt_server.Context;
using jwt_server.Models;
using jwt_server.Repositories.Interfaces;

namespace jwt_server.Repositories.Implements
{
    public class UserRepository: IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            this._context = context;
        }
        
        public User Create(User user)
        {
            this._context.Users.Add(user);
            user.Id = this._context.SaveChanges();
            return user;
        }

        public User GetByEmail(string email)
        {
            var user = this._context.Users.FirstOrDefault(u => u.Email == email);
            return user;
        }

        public User GetById(int Id)
        {
            var user = this._context.Users.FirstOrDefault(u => u.Id == Id);
            return user;
        }
    }
}