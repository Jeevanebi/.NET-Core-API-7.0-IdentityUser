using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Identity;

namespace WebService.API.Entity
{

    public class User : IdentityUser
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
    }
}
