using Microsoft.AspNetCore.Identity;

namespace InvestmentManager.ApplicationCore.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(IdentityUser user);
    }
}
