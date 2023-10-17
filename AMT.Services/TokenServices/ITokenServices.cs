
using System.Security.Claims;

namespace AMT.Services.TokenServices
{
    public interface ITokenServices
    {
        string SecretKey { get; }
        string Issuer { get; }
        string Audience { get; }
        int ExpiryInDays { get; }

        public string GenerateToken(GenerateTokenProperties properties);
        public ClaimsPrincipal ValidateToken(string token);
    }
}
