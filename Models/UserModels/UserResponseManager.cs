using Microsoft.AspNetCore.Identity;
using WebService.API.Entity;

namespace WebService.API.Models.UserModels
{
    public class UserResponseManager 
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
