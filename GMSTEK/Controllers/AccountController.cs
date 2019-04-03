using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using GMSTEK.Models;
using GMSTEK.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GMSTEK.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this._configuration = configuration;
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserInfo model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return BuildToken(model);
                }

                return BadRequest(new { message = "Password require at least one non-alfanumeric and one number and one uppercase char" });

            }

            return BadRequest(ModelState);


        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(userInfo.UserName, userInfo.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return BuildToken(userInfo);
                }
               
                //ModelState.AddModelError("error", "Invalid login attempt.");
                return BadRequest(new { message = "Invalid login attempt." });

            }

            return BadRequest(ModelState);

        }

        private IActionResult BuildToken(UserInfo userInfo)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["secretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: "mydomain.com",
               audience: "mydomain.com",
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration,
                userName = userInfo.UserName
            });

        }
    }
}
