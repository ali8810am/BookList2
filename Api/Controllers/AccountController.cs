using Api.ConstantParameters;
using Api.Data;
using Api.Models.Identity;
using Api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
     
        private readonly ILogger<AccountController> _logger;
        private readonly IAuthManager _authManager;

        public AccountController(ILogger<AccountController> logger, IAuthManager authManager)
        {
            _logger = logger;
            _authManager = authManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginDto)
        {

            _logger.LogInformation($"login attemped for {loginDto.UserName}");
            var response =await _authManager.Login(loginDto);
            return Ok(response);

        }
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<RegisterResponseDto>> Register([FromBody] RegisterRequestDto userDto)
        {
            _logger.LogInformation($"registration attemped for {userDto.FirstName}  {userDto.LastName}");
            var response = await _authManager.Register(userDto);
            return Ok(response);

        }

    }
}
