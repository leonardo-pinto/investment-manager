using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentManager.Web.Controllers
{
    [Route("auth")]
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

        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost]
        [Route("register")]
        async public Task<ActionResult<AuthResponse>> Register(RegisterRequest registerRequest)
        {
            IdentityResult? result = await _authService.Register(registerRequest);

            if (result.Succeeded)
            {
                IdentityUser? user = await _authService.FindUserByUserName(registerRequest.UserName);

                if (user is null)
                {
                    return Unauthorized(new ErrorResponse() { Error = "User not found" });
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
                List<string> errors = new List<string>();
                foreach (var error in result.Errors)
                {
                    errors.Add(error.Description);
                }

                return BadRequest(new ErrorResponse() { Error = string.Join(", ", errors)});
            }
        }

        /// <summary>
        /// Login an existing user
        /// </summary>
        [HttpPost]
        [Route("login")]
        async public Task<ActionResult<AuthResponse>> Login(LoginRequest loginRequest)
        {
            var result = await _authService.Login(loginRequest);

            if (!result.Succeeded)
            {
                return Unauthorized(new ErrorResponse() { Error = "Invalid credentials" });
            }

            IdentityUser? user = await _authService.FindUserByUserName(loginRequest.UserName);
            
            if (user is null)
            {
                return Unauthorized(new ErrorResponse() { Error = "User not found" });
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
