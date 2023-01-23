using Microsoft.AspNetCore.Identity;
using WebService.API.Entity;

namespace WebService.API.Models.UserModels
{
    public class UserResponseManager 
    {
        public bool IsSuccess { get; set; }
        public dynamic?  Message { get; set; }
        public IEnumerable<dynamic>? Errors { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
