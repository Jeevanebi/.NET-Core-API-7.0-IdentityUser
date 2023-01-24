using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebService.API.Models;
using WebService.API.Models.AuthModels;
using WebService.API.Models.UserModels;
using WebService.API.Services;

namespace WebService.API.Repository
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private UserManager<IdentityUser> _userManger;
        private IMailService _mailService;
        private readonly IUserService _useService;

        public AuthService(IConfiguration config, UserManager<IdentityUser> userManager, IMailService mailService, IUserService userService)
        {
            _config = config;
            _userManger = userManager;
            _mailService = mailService;
            _useService = userService;
        }

        //Register User
        public async Task<UserResponseManager> RegisterUser(RegisterUser model)
        {
            //if (model == null)
            //    throw new NullReferenceException("Data provided is NULL");

            //if (model.Password != model.ConfirmPassword)
            //    return new UserResponseManager
            //    {
            //        Message = "Confirm password doesn't match the password",
            //        IsSuccess = false,
            //    };
            var identityUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Username,
            };

            //var result = await _userManger.CreateAsync(identityUser, model.Password);
            var createdUser = await _useService.CreateUser(model);  

            //if (createdUser.IsSuccess == true)
            //{
            //    var confirmEmailToken = await _userManger.GenerateEmailConfirmationTokenAsync(identityUser);

            //    var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
            //    var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

            //    string url = $"{_config["AppUrl"]}/api/auth/confirmemail?userid={identityUser.Id}&token={validEmailToken}";

            //    await _mailService.SendEmailAsync(identityUser.Email, "Confirm your email", $"<h1>Welcome to Test API</h1>" +
            //        $"<p>Please confirm your email by <a href='{url}'>Clicking here</a></p>");


                return new UserResponseManager
                {
                    Message = "User created successfully! Please confirm the your Email!",
                    IsSuccess = true
                };
            //}

            //return new UserResponseManager
            //{
            //    Message = "User did not create",
            //    IsSuccess = false,
            //    Errors = createdUser.Errors.Select(e => e.message)
            //};

        }

        //Login User
        public async Task<UserResponseManager> LoginUser(AuthUser model)
        {
            var user = await _userManger.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new UserResponseManager
                {
                    Message = "There is no user with that Email address",
                    IsSuccess = false,
                };
            }

            var result = await _userManger.CheckPasswordAsync(user, model.Password);

            if (!result)
                return new UserResponseManager
                {
                    Message = "Invalid password",
                    IsSuccess = false,
                };
            var Token = GenerateToken(model, user.Id);

            return new UserResponseManager
            {
                Message = Token,
                IsSuccess = true,
                ExpireDate = DateTime.Now.AddHours(24)
            };
        }

        //ConfirmEmail
        public async Task<UserResponseManager> ConfirmEmail(string userId, string token)
        {
            var user = await _userManger.FindByIdAsync(userId);
            if (user == null)
                return new UserResponseManager
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            var decodedToken = WebEncoders.Base64UrlDecode(token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManger.ConfirmEmailAsync(user, normalToken);

            if (result.Succeeded)
                return new UserResponseManager
                {
                    Message = "Email confirmed successfully!",
                    IsSuccess = true,
                };

            return new UserResponseManager
            {
                IsSuccess = false,
                Message = "Email did not confirm",
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        //Forget Password
        public async Task<UserResponseManager> ForgetPassword(string email)
        {
            var user = await _userManger.FindByEmailAsync(email);
            if (user == null)
                return new UserResponseManager
                {
                    IsSuccess = false,
                    Message = "No user associated with email",
                };

            var token = await _userManger.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);

            string url = $"{_config["AppUrl"]}/ResetPassword?email={email}&token={validToken}";

            await _mailService.SendEmailAsync(email, "Reset Password", "<h1>Follow the instructions to reset your password</h1>" +
                $"<p>To reset your password <a href='{url}'>Click here</a></p>");

            return new UserResponseManager
            {
                IsSuccess = true,
                Message = "Reset password URL has been sent to the email successfully!"
            };
        }

        //Reset Password
        public async Task<UserResponseManager> ResetPassword(ResetPasswordModel model)
        {
            var user = await _userManger.FindByEmailAsync(model.Email);
            if (user == null)
                return new UserResponseManager
                {
                    IsSuccess = false,
                    Message = "No user associated with email",
                };

            if (model.NewPassword != model.ConfirmPassword)
                return new UserResponseManager
                {
                    IsSuccess = false,
                    Message = "Password doesn't match its confirmation",
                };

            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManger.ResetPasswordAsync(user, normalToken, model.NewPassword);

            if (result.Succeeded)
                return new UserResponseManager
                {
                    Message = "Password has been reset successfully!",
                    IsSuccess = true,
                };

            return new UserResponseManager
            {
                Message = "Something went wrong",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description),
            };
        }

        //Token Genereator
        private string GenerateToken(AuthUser user, string id)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var tokenClaims = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenClaims);
            return tokenString;
        }


        //Authenticate

        //private User Authenticate(AuthUser auth)
        //{


        //    if (string.IsNullOrEmpty(auth.UserName) || string.IsNullOrEmpty(auth.Email) || string.IsNullOrEmpty(auth.Password))
        //        return null;

        //    var user = _userManger.Users.SingleOrDefault(x => x.Email == auth.Email);

        //     check if username exists
        //    if (user == null)
        //        return null;
        //     check if password is correct
        //    if (!VerifyPasswordHash(auth.Password, user.PasswordHash, user.PasswordSalt))
        //        return null;

        //     authentication successful
        //    return null;
        //}



        //private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        //{
        //    if (password == null) throw new ArgumentNullException("password");
        //    if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
        //    if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
        //    if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

        //    using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
        //    {
        //        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        //        for (int i = 0; i < computedHash.Length; i++)
        //        {
        //            if (computedHash[i] != storedHash[i]) return false;
        //        }
        //    }

        //    return true;
        //}


    }
}
