using WebService.API.Entity;
using WebService.API.Models;
using WebService.API.Models.AuthModels;
using WebService.API.Models.UserModels;

namespace WebService.API.Repository
{
    public interface IAuthService
    {

        Task<UserResponseManager> RegisterUser(RegisterUser model);

        Task<UserResponseManager> LoginUser(AuthUser model);

        Task<UserResponseManager> ConfirmEmail(string userId, string token);

        Task<UserResponseManager> ForgetPassword(string email);

        Task<UserResponseManager> ResetPassword(ResetPasswordModel model);
    }
}
