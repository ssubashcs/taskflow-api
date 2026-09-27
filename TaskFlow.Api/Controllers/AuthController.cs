using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Dtos;
using TaskFlow.Api.Services;

namespace TaskFlow.Api.Controllers
{
    // [controller] is replaced by the controller's name without Controller.
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register(RegisterRequestDto registerRequest)
        {
            var user = await _authService.RegisterAsync(registerRequest);

            if (user is null) return Conflict("An account with this email already exists.");

            return StatusCode(StatusCodes.Status201Created, user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto loginRequest)
        {
            var loginResponse = await _authService.LoginAsync(loginRequest);

            if (loginResponse is null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(loginResponse);
        }
    }
}
