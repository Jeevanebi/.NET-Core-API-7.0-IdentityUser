using WebService.API.Entity;
using WebService.API.Models;
using WebService.API.Models.AuthModels;
using WebService.API.Models.UserModels;

namespace WebService.API.Services
{
    public interface IAuthService
    {

        Task<ResponseManager> RegisterUser(RegisterUser model);

        Task<ResponseManager> LoginUser(AuthUser model);

        Task<ResponseManager> ConfirmEmail(string userId, string token);

        Task<ResponseManager> ForgetPassword(string email);

        Task<ResponseManager> ResetPassword(ResetPasswordModel model);
    }
}
