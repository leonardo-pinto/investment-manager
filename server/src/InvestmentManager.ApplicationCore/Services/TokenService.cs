using InvestmentManager.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InvestmentManager.ApplicationCore.Services
{
    public class TokenService : ITokenService
    {
        // The JWT generation code was developed based on 
        // https://github.com/JacobToftgaardRasmussen/MediumJWTAuthenticationASPNETCoreAPI/tree/main/ApiWithAuth

        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private const int ExpirationMinutes = 600;

        public string GenerateToken(IdentityUser user)
        {
            DateTime expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);

            JwtSecurityToken token = CreateJwtToken(
                CreateClaims(user),
                CreateSigningCredentials(),
                expiration
            );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }

        private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials,
            DateTime expiration)
        {
            return new JwtSecurityToken(
                issuer: _configuration["JwtIssuer"],
                audience: _configuration["JwtAudience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
           );
        }

        private List<Claim> CreateClaims(IdentityUser user)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName), // represents the subject of the jwt
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // unique id of the jwt
            };
        }

        private SigningCredentials CreateSigningCredentials()
        {
            return new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecret"])),
                SecurityAlgorithms.HmacSha256
            );
        }
    }
}
