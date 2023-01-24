
using WebService.API.Data;
using WebService.API.Repository; 
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebService.API.Models.UserModels;
using WebService.API.Models;
using System.Data;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebService.API.Services
{
    public class UserService : IUserService
    {
        private readonly IdentityUserContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(IdentityUserContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<UserResponseManager> GetUsers() {
            var userAll = await _userManager.Users.ToListAsync();
            return new UserResponseManager
            {
                IsSuccess = true,
                Message = userAll
            };
        }

        public async Task<UserResponseManager> GetUserbyId(string id) {
            var userById = await _userManager.FindByIdAsync(id);
            return new UserResponseManager
            {
                IsSuccess = true,
                Message = userById
            };
        }

        public async Task<UserResponseManager> UpdateUser(string id, UpdateUser user)
        {
            if (user != null)
            {
                var findUser = await GetUserbyId(id);
                if (findUser != null) {

                    var updateUser = new IdentityUser
                    {
                        UserName = user.Username,
                        Email= user.Email,  
                        PhoneNumber= user.PhoneNo
                    };
                    try
                    {
                        /*context.Users.Add(findUser.)*/
                        
                        await _userManager.UpdateAsync(updateUser);
                        return new UserResponseManager
                        {
                            IsSuccess = true,
                            Message = user
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
                        IsSuccess= false,
                        Message= "User not found!"
                };

            }
            return new UserResponseManager() 
            { 
                IsSuccess= false,
                Message="updating property should not null!"
            };
 
        }

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
            var identityUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Username,
            };
            try {
                var result = await _userManager.CreateAsync(identityUser, model.Password);
                return new UserResponseManager
                {
                    IsSuccess = true,
                    Message = result
                };
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
    

            public bool IsExist(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

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

