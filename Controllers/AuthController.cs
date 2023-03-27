using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebService.API.Helpers;
using WebService.API.Models;
using WebService.API.Models.AuthModels;
using WebService.API.Models.UserModels;
using WebService.API.Services;

namespace WebService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly IConfiguration _config;

        public AuthController(IAuthService AuthService, IMapper mapper, IMailService mailService, IConfiguration configuration)
        {
            _auth = AuthService;
            _mapper = mapper;
            _mailService = mailService;
            _config = configuration;
        }

        // api/auth/Register
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterUser model)
        {
            if (ModelState.IsValid)
            {
                var result = await _auth.RegisterUser(model);

                if (result.IsSuccess)
                    return Ok(result); // Status Code: 200 

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid"); // Status code: 400
        }

        // api/auth/Authenticate
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromForm] AuthUser model)
        {
            if (ModelState.IsValid)
            {
                var result = await _auth.LoginUser(model);

                if (result.IsSuccess)
                {
                    var sub = "Detected - New login for "+model.Email;
                    var content = "<h1>Hey "+model.UserName +"!<br> New login to your account noticed</h1><strong><p>New login to your account at <h2>" + DateTime.Now + "</h2><br></p></strong><strong>Your Login token : </strong><code> "+ result.Message + "</code><p><br><h1>Expires :  "+DateTime.Now.AddHours(24)+"</h1>";
                    var mailContent = new MailRequest
                    {
                        ToEmail= model.Email,
                        Subject =  sub,
                        Body= content
                    };
                    await _mailService.SendEmailAsync(mailContent);
                    return Ok(new ResponseManager
                    {
                        IsSuccess= true,
                        Message = "We have sent you the Login Token to your registered Mail : "+model.Email+", Please use the security token to access the API(Authorize)!"
                    });
                }

                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");
        }

        // /api/auth/ConfirmEmail?userid&token
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return NotFound();

            var result = await _auth.ConfirmEmail(userId, token);

            if (result.IsSuccess)
            {
                return Content("Email Verified Successfully!");
            }

            return BadRequest(result);
        }

        // api/auth/ForgetPassword
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return NotFound();

            var result = await _auth.ForgetPassword(email);

            if (result.IsSuccess)
                return Ok(result); // 200

            return BadRequest(result); // 400
        }

        // api/auth/ResetPassword
        [HttpPost("ResetPassword")]
        //[Description("This Method will not work in Swagger, Check your Registered Mail to get the Request link, Make a request using PostMan or any API Testing Tools with the Request Link")]
        public async Task<IActionResult> ResetPassword([FromQuery]ResetPasswordModel model)
        {
            
            if (ModelState.IsValid)
            {
                var result = await _auth.ResetPassword(model);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid");
        }


    }
}
