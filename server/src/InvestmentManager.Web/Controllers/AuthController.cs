using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentManager.Web.Controllers
{
    [Route("api/auth")]
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        async public Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult? result = await _authService.Register(registerRequest);

            if (result.Succeeded)
            {
                IdentityUser? user = await _authService.FindUserByUserName(registerRequest.UserName);

                if (user is null)
                {
                    return Unauthorized();
                }

                string accessToken = _tokenService.GenerateToken(user);

                return Ok(new AuthResponse
                {
                    Id = user.Id,
                    Username = registerRequest.UserName,
                    AccessToken = accessToken
                });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }
                return BadRequest(ModelState);
            }
        }

        [HttpPost("login")]
        async public Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.Login(loginRequest);

            if (!result.Succeeded)
            {
                return Unauthorized("Invalid credentials");
            }

            IdentityUser? user = await _authService.FindUserByUserName(loginRequest.UserName);
            
            if (user is null)
            {
                return Unauthorized();
            }

            string accessToken = _tokenService.GenerateToken(user);

            return Ok(new AuthResponse
            {
                Id = user.Id,
                Username = loginRequest.UserName,
                AccessToken = accessToken
            });
        }
    }
}
