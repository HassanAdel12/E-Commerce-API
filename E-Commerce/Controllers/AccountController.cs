using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using E_Commerce.Data;
using E_Commerce.DTO;
using E_Commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly UserManager<User> usermanger;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<User> _usermanger,
            RoleManager<IdentityRole> _roleManager,
            IConfiguration _config,
            IMapper _mapper)
        {
            mapper = _mapper;
            usermanger = _usermanger;
            config = _config;
            roleManager = _roleManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] CreateUser model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User existingUser = await usermanger.FindByEmailAsync(model.Email);

                    if (existingUser != null)
                    {
                        return BadRequest("Email address is already in use.");
                    }

                    User user = mapper.Map<User>(model);

                    IdentityResult result = await usermanger.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        return Ok();
                    }

                    string Error = string.Empty;
                    foreach (var error in result.Errors)
                        Error += $"{error.Description} , ";

                    return BadRequest(Error);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
                

            }
            return BadRequest(ModelState);
        }




        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUser model)
        {
            if (ModelState.IsValid == true)
            {

                User user = await usermanger.FindByEmailAsync(model.Email);


                if (user is null || !await usermanger.CheckPasswordAsync(user, model.Password))
                {
                    return Unauthorized();
                }


                user.LastLoginTime = DateTime.Now;

                JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
                var roles = await usermanger.GetRolesAsync(user);
                
                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    ExpireOn = jwtSecurityToken.ValidTo,
                    Roles = roles.ToList(),
                    Email = model.Email,
                    UserName = user.UserName,
                    LastLoginTime = user.LastLoginTime

                });

            }
            return Unauthorized();

        }

        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var userClaims = await usermanger.GetClaimsAsync(user);
            var roles = await usermanger.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim(ClaimTypes.Role, role));

            var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("LastLoginTime", user.LastLoginTime.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }
                .Union(userClaims)
                .Union(roleClaims);

            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(config["JWT:Key"]));

            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey,
                SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: config["JWT:Issuer"],
                audience: config["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

    }



}
