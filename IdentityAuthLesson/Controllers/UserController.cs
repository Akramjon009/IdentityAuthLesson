using IdentityAuthLesson.DTOs;
using IdentityAuthLesson.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityAuthLesson.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AppUser
            {
                FullName = registerDTO.FullName,
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                Age = registerDTO.Age,
                Status = registerDTO.Status
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            foreach (var role in registerDTO.Roles)
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<string>> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if(user is null)
            {
                return Unauthorized("User not Found with this email");
            }

            var test = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (!test)
            {
                return Unauthorized("Password invalid");
            }

            return Ok("Welcome to the world");
        }

        [HttpGet]
        public async Task<ActionResult<string>> GetAllUsers()
        {
            var result = await _userManager.Users.ToListAsync();

            if (result is null)
            {
                return Unauthorized("Not Users");
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<string>> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
            {
                return Unauthorized("User not Found with this id");
            }

            return Ok(user);
        }
    }
}
