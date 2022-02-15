using LibApp.Data;
using LibApp.Dtos;
using LibApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LibApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ICustomerRepository _customerRepository;

        public AccountsController(IAccountService accountService, ICustomerRepository customerRepository)
        {
            _accountService = accountService;
            _customerRepository = customerRepository;
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

        [HttpPost("validate/{email}")]
        public ActionResult ValidateJWTToken(string email)
        {
            var customer = _customerRepository.GetAllCustomersWithMembershipType()
                .Where(c => c.Email == email)
                .FirstOrDefault();

            if(customer == null)
            {
                return Ok(false);
            }

            return Ok(true);
        }
    }
}
