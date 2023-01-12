using Microsoft.AspNetCore.Identity;
using WebService.API.Entity;
using WebService.API.Models;
using WebService.API.Models.UserModels;

namespace WebService.API.Repository
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserbyId(int id);
        Task<UserResponseManager> RegisterUserAsync(RegisterUser model);
        Task<UserResponseManager> PutUser(User model);
        Task<UserResponseManager> DeleteUser(User user);
        Task<bool> IsExist(int id);
        
    }
}
