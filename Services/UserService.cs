
using WebService.API.Data;
using WebService.API.Entity;
using WebService.API.Repository;
using Microsoft.EntityFrameworkCore;
using WebService.API.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using WebService.API.Models.UserModels;
using WebService.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebService.API.Services
{
    public class UserService : IUserService
    {
        private readonly IdentityUserContext _context;
        private UserManager<IdentityUser> _userManager;
        private  IAuthService _authService;
        private readonly IMapper _mapper;

        public UserService(IdentityUserContext context, UserManager<IdentityUser> userManager, IAuthService authService, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _authService =authService;
            _mapper = mapper;
        }

        //public async IEnumerable<User> GetUsers() => await _userManager.Users.ToList();

        //public User PostUser(User createUser, string Password)
        //{
        //    if (_context.Users.Any(x => x.Email == createUser.Email))
        //        throw new AppException("User Email \"" + createUser.Email + "\" is already taken");

        //    byte[] passwordHash, passwordSalt;
        //    CreatePasswordHash(Password, out passwordHash, out passwordSalt);

        //    createUser.PasswordHash = passwordHash;
        //    createUser.PasswordSalt = passwordSalt;

        //    var identityUser = new IdentityUser
        //    {
        //        Email = createUser.Email,
        //        UserName = createUser.Username,
        //        //PhoneNumber = createUser.PhoneNo,
        //        //PhoneNumberConfirmed = true
        //        //PasswordHash = Convert.ToString(createUser.PasswordHash),
        //        //SecurityStamp = Convert.ToString(createUser.PasswordSalt),
        //    };
        //    //var role = new IdentityRole
        //    //{
        //    //    Id = identityUser.Id,
        //    //    Name = identityUser.UserName,
        //    //    NormalizedName = identityUser.UserName,
        //    //    ConcurrencyStamp = Convert.ToString(DateTime.Now) 
        //    //};
        //    var result = _userManager.CreateAsync(identityUser);

        //    if (result.IsCompleted)
        //    {
        //        return createUser;
        //    }
        //    return null;
        //}



        public void DeleteUser(User user)
        {
            //_context.Users.Remove(user);
            _context.SaveChanges();

        }

        void IUserService.PutUser(int id, UpdateUser user)
        {
            var updateobj = _context.Users.Find(id);
            //updateobj.Username = user.Username;
            //updateobj.Email = user.Email;
            //updateobj.PhoneNo = user.PhoneNo;

            _context.SaveChanges();
        }
       

        //private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        //{
        //    if (password == null) throw new ArgumentNullException("password");
        //    if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

        //    using (var hmac = new System.Security.Cryptography.HMACSHA512())
        //    {
        //        passwordSalt = hmac.Key;
        //        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        //    }
        //}

        //public async Task<IEnumerable<User>> GetUsers()
        //{
        //    var userStore = new UserStore<IdentityUser>(IdentityUserContext);
        //    var userManager = new UserManager<IdentityUser>(userStore);
        //    IQueryable<IdentityUser> usersQuery = userManager.Users;
        //    List<IdentityUser> users = usersQuery.ToList();
        //}

        public async Task<User> GetUserbyId(string id)
        {
            var userById = await _userManager.GetUserId(id);
            return userById;
        }

        public async Task<UserResponseManager> RegisterUser(RegisterUser model)
        {
            var PostUser = await _authService.RegisterUser(model);
            if (PostUser != null)
            {
                return PostUser;
            }
            return new UserResponseManager
            {
                Message = "User Creating Failed",
                IsSuccess = false,
                Errors = PostUser.Errors,
            };
        }

        public Task PutUser(User model)
        {
            throw new NotImplementedException();
        }

        Task IUserService.DeleteUser(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsExist(String id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return true;
            }
            return false;

        }

        public Task<User> RegisterUserAsync(RegisterUser model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsExist(int id)
        {
            throw new NotImplementedException();
        }
    }
}

