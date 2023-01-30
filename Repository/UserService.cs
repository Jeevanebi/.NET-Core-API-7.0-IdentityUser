using WebService.API.Data;
using WebService.API.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebService.API.Models.UserModels;
using WebService.API.Models;
using System.Data;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using WebService.API.Helpers;

namespace WebService.API.Services
{
    public class UserService : IUserService
    {
        private readonly IdentityUserContext _context;
        private readonly IConfiguration _config;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMailService _mailService;

        public UserService(IdentityUserContext context,
                           UserManager<IdentityUser> userManager,
                           RoleManager<IdentityRole> roleManager,
                           IMailService mailService,
                           IConfiguration config)
        {
            _context = context;
            _config = config;
            _userManager = userManager;
            _roleManager = roleManager;
            _mailService = mailService;
        }

        //All Users
        public async Task<UserResponseManager> GetUsers()
        {
            var userAll = await _userManager.Users.ToListAsync();
            return new UserResponseManager
            {
                IsSuccess = true,
                Message = userAll
            };
        }

        //GetUserByID
        public async Task<UserResponseManager> GetUserbyId(string id)
        {
            var userById = await _userManager.FindByIdAsync(id);
            return new UserResponseManager
            {
                IsSuccess = true,
                Message = userById
            };
        }

        //Create User
        public async Task<UserResponseManager> CreateUser(RegisterUser model)
        {
            if (model == null)
                throw new NullReferenceException("Data provided is NULL");

            if (model.Password != model.ConfirmPassword)
                return new UserResponseManager
                {
                    Message = "Confirm password doesn't match the password",
                    IsSuccess = false,
                };

            //Is User Exist
            var userFound = await _userManager.FindByEmailAsync(model.Email);

            //-Not Exists
            if (userFound == null)
            {
                var identityUser = new IdentityUser
                {
                    Email = model.Email,
                    UserName = model.Username,
                    NormalizedUserName = model.Username.Normalize(),
                    NormalizedEmail = model.Email.Normalize(),
                    PhoneNumber = model.PhoneNumber

                };

                try
                {
                    var result = await _userManager.CreateAsync(identityUser, model.Password);


                    //    //if (model.Role != null)
                    //    //{
                    //    //    await _userManager.AddToRoleAsync(identityUser, Convert.ToString(model.Role));
                    //    //}
                    //    //else
                    //    //{
                    //    //    await _userManager.AddToRoleAsync(identityUser, Convert.ToString("Guest"));
                    //    //}

                    return new UserResponseManager
                    {
                        IsSuccess = true,
                        Message = result
                    };
                    //}
                }

                catch (DBConcurrencyException ex)
                {
                    return new UserResponseManager()
                    {
                        IsSuccess = false,
                        Message = ex.Message
                    };
                }
            }
            //- User Exist
            return new UserResponseManager
            {
                IsSuccess = false
            };

        }

        //Update User
        public async Task<UserResponseManager> UpdateUser(string id, UpdateUser user)
        {
            if (user != null)
            {
                var findUser = await _userManager.FindByIdAsync(id);
                if (findUser != null)
                {

                    try
                    {
                        /*context.Users.Add(findUser.)*/
                        var updateUser = new IdentityUser
                        {
                            UserName = user.Username,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNo
                        };
                        var up = await _userManager.UpdateAsync(updateUser);
                        var updatedUser = await _userManager.FindByIdAsync(updateUser.Id);
                        var updatedUserResponse = new IdentityUser
                        {
                            Id = id,
                            UserName = updatedUser.UserName,
                            NormalizedUserName = updatedUser.Email,
                            Email = updatedUser.Email,
                            NormalizedEmail = updatedUser.Email,
                            PhoneNumber = updatedUser.PhoneNumber,

                        };
                        return new UserResponseManager
                        {
                            IsSuccess = true,
                            Message = updatedUserResponse
                        };

                    }
                    catch (Exception ex)
                    {

                        return new UserResponseManager
                        {
                            IsSuccess = false,
                            Message = ex.Message
                        };
                    }
                }
                return new UserResponseManager()
                {
                    IsSuccess = false,
                    Message = "User not found!"
                };

            }
            return new UserResponseManager()
            {
                IsSuccess = false,
                Message = "updating property should not null!"
            };

        }

        //Delete User
        public async Task<UserResponseManager> DeleteUser(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            try
            {
                await _userManager.DeleteAsync(user);

                return new UserResponseManager
                {
                    IsSuccess = true,
                    Message = " User " + id + " removed successfully!"
                };
            }
            catch
            {
                throw;
            }
            return null;

        }

        //User Exist
        public bool IsExist(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        //Additional Creating Password Hash
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}

