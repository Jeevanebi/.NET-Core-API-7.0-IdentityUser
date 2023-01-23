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

        //Task<UserResponseManager> UpdateUser(UpdateUser create, string Password);
        //Task<UserResponseManager> DeleteUser(int id);
        public bool IsExist(string id);

    }
}
