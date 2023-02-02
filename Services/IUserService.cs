using Microsoft.AspNetCore.Identity;
using WebService.API.Entity;
using WebService.API.Models;
using WebService.API.Models.UserModels;

namespace WebService.API.Repository
{
    public interface IUserService
    {
        Task<ResponseManager> GetUsers();
        Task<ResponseManager> GetUserbyId(string id);
        Task<ResponseManager> CreateUser(RegisterUser model);

        Task<ResponseManager> UpdateUser(string id, UpdateUser user);
        Task<ResponseManager> DeleteUser(string id);
        public bool IsExist(string id);

    }
}
