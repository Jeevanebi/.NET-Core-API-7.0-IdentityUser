using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebService.API.Data;
using WebService.API.Entity;
using WebService.API.Models;
using WebService.API.Models.UserModels;
using WebService.API.Repository;

namespace WebService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _user;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _user = userService;
            _mapper = mapper;
        }

        // GET: api/Users

        [HttpGet]
        //[Authorize(Roles = "SuperAdmin")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUsers() 
        {
            var AllUser = await _user.GetUsers();
            return Ok(AllUser);
        }
        [AllowAnonymous]
        // GET: api/Users/5
        [HttpGet("{id}")]
        //[Authorize(Roles = "SuperAdmin, Admin, Agent")]
        public async Task<IActionResult> GetUserbyId(string id)
        {
            var userById = await _user.GetUserbyId(id);

            if (userById == null)
            {
                return NotFound("User for the $`{id}` not found!");
            }

            return Ok(userById);
        }

        [AllowAnonymous]
        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, UpdateUser user)
        {
            if (user != null)
            {
                var updateUser = await _user.GetUserbyId(id);
                if(updateUser!= null) {
                    var userUpdated = await _user.UpdateUser(id, user);
                    return Ok(userUpdated);
                }
            }
            return BadRequest();
            
        }

        [AllowAnonymous]
        // POST: api/Users
        [HttpPost]
        public IActionResult PostUser([FromBody] RegisterUser user)
        {
            var model = _mapper.Map<User>(user);
            var createUser = _user.CreateUser(user);
            return Ok(createUser);
        }

        [AllowAnonymous]
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        //[Authorize(Roles = "SuperAdmin")]

        public IActionResult DeleteUser(string id)
        {
            var user = _user.GetUserbyId(id);
            if (user == null)
            {
                return NotFound("User Not Found");
            }

            _user.DeleteUser(id);
            return NotFound("User Deleted");
        }

        private bool UserExists(string id)
        {
            return _user.IsExist(id);
        }
    }
}
