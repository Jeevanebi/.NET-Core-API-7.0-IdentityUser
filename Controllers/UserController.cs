using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebService.API.Data;
using WebService.API.Entity;
using WebService.API.Models;
using WebService.API.Repository;

namespace WebService.API.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _user;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, ApplicationDbContext context, IMapper mapper)
        {
            _user = userService;
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Users

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult GetUsers() 
        {
            var AllUser = _user.GetUsers();
            return Ok(AllUser);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin, Admin, Agent")]
        public IActionResult GetUserbyId(int id)
        {
            var userById = _user.GetUserbyId(id);

            if (userById == null)
            {
                return NotFound("User for the $`{id}` not found!");
            }

            return Ok(userById);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public IActionResult PutUser(int id, UpdateUser user)
        {
            var dbuserid = _context.Users.Find(id);
            if (id != dbuserid.Userid)
            {
                return NotFound("Error : Invalid Put Request, User Not Found !");
            }

            try
            {
                _user.PutUser(id, user);
            }


            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound("Error Updating the User !");
                }
                else
                {
                    throw;
                }
            }

            return Ok("Success !");
        }

        // POST: api/Users
        [HttpPost]
        [AllowAnonymous]
        public IActionResult PostUser([FromBody] RegisterUser user)
        {
            var model = _mapper.Map<User>(user);
            var createUser = _user.PostUser(model,user.Password);
            return Ok(createUser);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult DeleteUser(int id)
        {
            var user = _user.GetUserbyId(id);
            if (user == null)
            {
                return NotFound("User Not Found");
            }

            _user.DeleteUser(user);
            return NotFound("User Deleted");
        }

        private bool UserExists(int id)
        {
            return _user.IsExist(id);
        }
    }
}
