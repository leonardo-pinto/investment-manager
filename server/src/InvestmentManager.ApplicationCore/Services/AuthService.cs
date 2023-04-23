using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace InvestmentManager.ApplicationCore.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> Register(RegisterRequest registerRequest)
        {
            var user = new IdentityUser { UserName = registerRequest.UserName };
            return await _userManager.CreateAsync(
                user,
                registerRequest.Password
            );
        }

        public async Task<SignInResult> Login(LoginRequest loginRequest)
        {
            IdentityUser user = new() { UserName = loginRequest.UserName };
            return await _signInManager.PasswordSignInAsync(user.UserName, loginRequest.Password, false, false);
        }

        public async Task<IdentityUser?> FindUserByUserName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }
    }
}
