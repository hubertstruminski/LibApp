using LibApp.Dtos;
using LibApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] RegisterUserDto registerDto)
        {
            _accountService.RegisterUser(registerDto);
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginUserDto loginDto)
        {
            string token = _accountService.GenerateJWT(loginDto);
            return Ok(token);
        }
    }
}
