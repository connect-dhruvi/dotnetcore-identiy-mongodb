using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using slotmachine_api.Models.Identity;
using slotmachine_api.Models.Requests;
using slotmachine_api.Models.Responses;
using slotmachine_api.Models.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace slotmachine_api.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtSettings _jwtSettings;

        public UserController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtSettings jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings;
        }

        protected string BuildToken(ApplicationUser user)
        {
            var email = user.Email;
            if (string.IsNullOrEmpty(email))
                return null;

            var expires = DateTime.Now.AddMonths(1);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(_jwtSettings.JwtIssuer,
                _jwtSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // GET api/user/userdata
        [Authorize]
        [HttpGet]
        public IActionResult UserData()
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == new Guid(User.Identity.Name));
            var userData = new UserDataResponse
            {
                Name = user.UserName,
                LastName = user.LastName,
                City = user.City,
                Email = user.Email,
                Points =user.Points
            };
            return Ok(userData);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterEntity model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Name = model.Name,
                    LastName = model.LastName,
                    City = model.City,
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    var token = BuildToken(user);
                    var rootData = new SignUpResponse(token, user.UserName, user.Email);
                    return Created("api/v1/user/register", rootData);
                }
                return Ok(string.Join(",", result.Errors?.Select(error => error.Description)));
            }
            string errorMessage = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return BadRequest(errorMessage ?? "Bad Request");
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody]LoginEntity model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                    var token = BuildToken(appUser);

                    var rootData = new LoginResponse(token, appUser.UserName, appUser.Email);
                    return Ok(rootData);
                }
                return StatusCode((int)HttpStatusCode.Unauthorized, "Bad Credentials");
            }
            string errorMessage = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return BadRequest(errorMessage ?? "Bad Request");
        }
    }
}

/*
 //api/v1/user/register
//Request
{
    "Email": "demo@gmail.com",
    "Password": "Test1234!",
    "ConfirmPassword": "Test1234!",
    "Name": "Alejandro",
    "LastName": "Ruiz",
    "City": "Guadalajara"
}

//Response
{
    "token": "token",
    "userName": "demo@gmail.com",
    "email": "demo@gmail.com"
}

//api/v1/user/login
//Request
{
	"Email":"alex@gmail.com",
	"Password":"Test1234!"
}

//Response
{
    "token": "token",
    "userName": "alex@gmail.com",
    "email": "alex@gmail.com"
}

//api/v1/user/userdata
//Request
//Header
//Authorization: Bearer yourToken
//Response
{
    "userName": "demo@gmail.com",
    "name": "",
    "lastName": "",
    "city": null,
    "email": "demo@gmail.com"
}
 */