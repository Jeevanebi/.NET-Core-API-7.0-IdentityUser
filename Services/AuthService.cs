using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebService.API.Data;
using WebService.API.Entity;
using WebService.API.Models;
using WebService.API.Repository;

namespace WebService.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public AuthService(IConfiguration config, ApplicationDbContext context)
        {
            _config = config;
            _context = context;
        }

        public User Authenticate(AuthUser auth)
        {

            //var _user = _context.Users.FindAsync(username);
            //var _password = _context.Users.FindAsync(password);
            //var currentUser = _context.Users.Find(auth.Email);

            //if (currentUser != null)
            //{
            //    if(currentUser.Username == auth.UserName && currentUser.Email == auth.Email)
            //    {
            //        return currentUser;
            //    }
            //}
            //return null;

            if (string.IsNullOrEmpty(auth.UserName) || string.IsNullOrEmpty(auth.Email) || string.IsNullOrEmpty(auth.Password))
                return null;

            var user = _context.Users.SingleOrDefault(x => x.Email == auth.Email);

            // check if username exists
            if (user == null)
                return null;

            //// check if password is correct
            //if (!VerifyPasswordHash(auth.Password, user.PasswordHash, user.PasswordSalt))
            //    return null;

            // authentication successful
            return user;
        }

        public string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(100),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
