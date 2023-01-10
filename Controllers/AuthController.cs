using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebService.API.Entity;
using WebService.API.Models;
using WebService.API.Repository;
using WebService.API.Services;

namespace WebService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly IUserService _userservice;
        private readonly IMapper _mapper;

        public AuthController(IAuthService AuthService, IUserService userService, IMapper mapper)
        {
            _auth = AuthService;
            _userservice = userService;
            _mapper = mapper;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Authentication")]
        public IActionResult Post([FromBody] AuthUser authentication)
        {
            var user = _auth.Authenticate(authentication);

            if (user != null)
            {
                var token = _auth.Generate(user);

                return Ok(new
                {
                    Id = user.Userid,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    Phone = user.PhoneNo,
                    Created_at = DateTime.UtcNow,
                    Token = token
                });

            }
            return NotFound("User Not Found");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public IActionResult createUser([FromBody] RegisterUser user)
        {
            var model = _mapper.Map<User>(user);
            var createUser = _userservice.PostUser(model, user.Password);
            return Ok(createUser);
        }
    }
}
