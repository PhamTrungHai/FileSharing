using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SaritasaT.Services;
using SaritasaT.Models;
using Microsoft.AspNetCore.Authorization;
using SaritasaT.DTOs.User;
using System.Text.Json;

namespace SaritasaT.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(
            IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Profile(int id)
        {
            var user = await _userService.GetById(id);
            return Ok(user);
        }

        [HttpPost(nameof(SignIn))]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(SignInModel user)
        {
            var newUser = await _userService.Authenticate(user.Email, user.Password);
            if(newUser == null)
            {
            return BadRequest(new {message="Username or password is incorrect"});
            }
            string token = _userService.GenerateUserToken(newUser);
            return Ok(JsonSerializer.Serialize(new{id=newUser.Id,name=newUser.Name,email=newUser.Email, token=token}));
        }

        [HttpPost(nameof(SignUp))]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            User user=new()
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
            };
            var newUser=await _userService.Create(user);
            return CreatedAtAction("Profile",new {id= newUser.Id},new { message = "User created" });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, User user)
        {
            _userService.Update(id, user);
            return Ok(new { message = "User updated" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok(new { message = "User deleted" });
        }
    }
}
