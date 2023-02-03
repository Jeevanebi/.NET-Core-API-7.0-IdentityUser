using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebService.API.Authorization;
using WebService.API.Models.UserModels;
using WebService.API.Repository;
using WebService.API.Services;

namespace WebService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _user;
        private readonly IMapper _mapper;
        private readonly IAuthService _auth;

        public UserController(IUserService userService, IMapper mapper, IAuthService authService)
        {
            _user = userService;
            _mapper = mapper;
            _auth = authService;
        }

        // GET: api/Users
        [HttpGet]
        //[Authorize(Roles = "SuperAdmin,Admin")]
        [Authorize(Permissions.Users.SuperAdminView)]
        //[AllowAnonymous]
        public async Task<IActionResult> GetUsers() 
        {
            var AllUser = await _user.GetUsers();
            return Ok(AllUser);
        }

        //[AllowAnonymous]
        // GET: api/Users/5
        [HttpGet("{id}")]
        //[Authorize(Roles = "SuperAdmin, Admin, Agent")]
        [Authorize(Permissions.Users.viewById)]
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
        //[Authorize(Roles = "SuperAdmin, Admin")]
        [Authorize(Permissions.Users.Edit)]
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
        [Authorize(Permissions.Users.Create)]
        //[Authorize(Permissions.Users.Create)]
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] RegisterUser user)
        {
            if (ModelState.IsValid)
            {
                var result = await _auth.RegisterUser(user);

                if (result.IsSuccess)
                    return Ok(result); // Status Code: 200 

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid"); // Status code: 400
        }

        // DELETE: api/Users/5
        //[AllowAnonymous]
        //[Authorize(Roles = "SuperAdmin")]
        [Authorize(Permissions.Users.Delete)]
        [HttpDelete("{id}")]
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
