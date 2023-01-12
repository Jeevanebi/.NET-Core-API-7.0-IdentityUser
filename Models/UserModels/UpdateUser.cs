using System.ComponentModel;
using WebService.API.Entity;

namespace WebService.API.Models.UserModels
{
    public class UpdateUser
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PhoneNo { get; set; }
    }
}
