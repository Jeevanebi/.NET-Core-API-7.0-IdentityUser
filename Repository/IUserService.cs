using WebService.API.Entity;
using WebService.API.Models;

namespace WebService.API.Repository
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        User GetUserbyId(int id);
        void PutUser(int id, UpdateUser user);
        User PostUser(User create, string Password);
        void DeleteUser(User user);
        public bool IsExist(int id);
        
    }
}
