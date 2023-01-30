using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [Authorize(Roles = "SuperAdmin")]
        //[AllowAnonymous]
        public async Task<IActionResult> GetUsers() 
        {
            var AllUser = await _user.GetUsers();
            return Ok(AllUser);
        }

        //[AllowAnonymous]
        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin, Admin, Agent")]
        public async Task<IActionResult> GetUserbyId(string id)
        {
            var userById = await _user.GetUserbyId(id);

            if (userById == null)
            {
                return NotFound("User for the $`{id}` not found!");
            }

            return Ok(userById);
        }

        //[AllowAnonymous]
        // PUT: api/Users/5
        [Authorize(Roles = "SuperAdmin, Admin")]
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


        // POST: api/Users
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] RegisterUser user)
        {
            //var model = _mapper.Map<User>(user);
            var createUser = await  _user.CreateUser(user);
            return Ok(createUser);
        }

        // DELETE: api/Users/5
        //[AllowAnonymous]
        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{id}")]
        //[Authorize(Roles = "SuperAdmin")]

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _user.GetUserbyId(id);
            if (user == null)
            {
                return NotFound("User Not Found");
            }

            await _user.DeleteUser(id);
            return Content("User Deleted");
        }

        private bool UserExists(string id)
        {
            return _user.IsExist(id);
        }
    }
}
