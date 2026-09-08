using clinicAPIsSystem.DTOs.AuthDTO;
using clinicAPIsSystem.IServices.IUserServices;
using Microsoft.AspNetCore.Mvc;

namespace clinicAPIsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(
            [FromBody] LoginDto loginDto)
        {
            var token = await _loginService.LoginAsync(loginDto);

            return Ok(token);
        }
    }
}