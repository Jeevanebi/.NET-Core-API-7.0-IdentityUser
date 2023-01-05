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

        public User? Authenticate(AuthUser auth)
        {

            //var _user = _context.Users.FindAsync(username);
            //var _password = _context.Users.FindAsync(password);

            var currentUser = _context.Users.FirstOrDefault(x =>
               x.Username.ToLower() == auth.UserName.ToLower()
            && x.Password == auth.Password);
            //&& x.Email.ToLower() == userLogin.Emailaddress.ToLower()


            if (currentUser != null)
            {
                return currentUser;
            }

            return null;
        }

        public string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                //new Claim(ClaimTypes.Role, user.Role)
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
