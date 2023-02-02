using Microsoft.AspNetCore.Identity;
using WebService.API.Entity;

namespace WebService.API.Models
{
    public class ResponseManager
    {
        public bool IsSuccess { get; set; }
        public dynamic? Message { get; set; }
        public IEnumerable<dynamic>? Errors { get; set; }

    }
}
