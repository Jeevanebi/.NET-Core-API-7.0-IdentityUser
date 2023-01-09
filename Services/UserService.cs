
using WebService.API.Data;
using WebService.API.Entity;
using WebService.API.Repository; 
using Microsoft.EntityFrameworkCore;
using WebService.API.Models;
using WebService.API.Helpers;

namespace WebService.API.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetUsers() => _context.Users.ToList();

        public User GetUserbyId(int id) => _context.Users.Find(id);


        public User PostUser(User createUser)
        {
            _context.Users.Add(createUser);
            _context.SaveChanges();
            return createUser;
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();

        }

        void IUserService.PutUser(int id, UpdateUser user)
        {
            var updateobj = _context.Users.Find(id);
            updateobj.Username = user.Username;
            updateobj.Email = user.Email;
            updateobj.PhoneNo = user.PhoneNo;

            _context.SaveChanges();
        }
        public bool IsExist(int id)
        {
            return _context.Users.Any(e => e.Userid == id);
        }
    }
}

