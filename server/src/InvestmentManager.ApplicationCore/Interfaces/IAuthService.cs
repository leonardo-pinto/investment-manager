using InvestmentManager.ApplicationCore.DTO;
using Microsoft.AspNetCore.Identity;

namespace InvestmentManager.ApplicationCore.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> Register(RegisterRequest registerRequest);
        Task<SignInResult> Login(LoginRequest loginRequest);
        Task<IdentityUser?> FindUserByUserName(string userName);
    }
}
