using Microsoft.AspNetCore.Identity;
using WebService.API.Entity;
using WebService.API.Models;
using WebService.API.Models.UserModels;

namespace WebService.API.Repository
{
    public interface IUserService
    {
        Task<UserResponseManager> GetUsers();
        Task<UserResponseManager> GetUserbyId(string id);
        Task<UserResponseManager> CreateUser(RegisterUser model);

        Task<UserResponseManager> UpdateUser(string id, UpdateUser user);
        Task<UserResponseManager> DeleteUser(string id);
        public bool IsExist(string id);

    }
}
