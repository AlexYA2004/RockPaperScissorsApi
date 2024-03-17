using Microsoft.AspNetCore.Mvc;
using RockPaperScissorsApi.Entities.Models;
using RockPaperScissorsApi.Services.Interfaces;

namespace RockPaperScissorsApi.Controllers
{
    [ApiController]
    [Route("RockPaperScissors/[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserLoginModel model)
        {
            var result = await _userService.Login(model);

            if (!result.Succeeded)
                return BadRequest();

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserRegisterModel model)
        {
            var result = await _userService.Register(model);

            if (!result.Succeeded)
                return BadRequest();

            return Ok(result);
        }

        [HttpPut("logout")]
        public async Task Logout()
        {
            await _userService.Logout();
        }
    }
}
