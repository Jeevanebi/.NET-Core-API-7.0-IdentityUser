using System.ComponentModel.DataAnnotations;

namespace WebService.API.Models
{
    public class AuthUser
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public string? Role { get; set; }
    }
}
