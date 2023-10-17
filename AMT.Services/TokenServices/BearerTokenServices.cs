
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AMT.Services.TokenServices
{
    public class BearerTokenServices : ITokenServices
    {
        public string SecretKey {get; private set;}

        public string Issuer { get; private set; }

        public string Audience { get; private set; }

        public int ExpiryInDays { get; private set; }

        public BearerTokenServices(IConfiguration config)
        {
            SecretKey = config["Jwt:Key"];
            Issuer = config["Jwt:Issuer"];
            Audience = config["Jwt:Audience"];
            ExpiryInDays = int.Parse(config["Jwt:ExpiryInDays"]);
        }

        public string GenerateToken(GenerateTokenProperties properties)
        {
            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, properties.UserId),
                new Claim(ClaimTypes.Name, properties.Name)
            };
            claims.AddRange(properties.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(ExpiryInDays),
                SigningCredentials = credentials,
                Issuer = Issuer,
                Audience = Audience
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(SecretKey));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = true,
                ValidIssuer = Issuer,
                ValidateAudience = true,
                ValidAudience = Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            return principal;
        }
    }
}
