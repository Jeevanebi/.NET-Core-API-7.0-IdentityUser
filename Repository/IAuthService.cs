using WebService.API.Entity;
using WebService.API.Models;

namespace WebService.API.Repository
{
    public interface IAuthService
    {
        User Authenticate(AuthUser auth);

        string Generate(User user);
    }
}
