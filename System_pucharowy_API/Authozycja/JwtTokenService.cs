using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System_pucharowy_API.Domain;

namespace System_pucharowy_API.Authozycja
{
    public class JwtTokenService
    {
        private readonly JWTOptions _opt;
        public JwtTokenService(IOptions<JWTOptions> opt)
        {
            _opt = opt.Value;
        }
        public string CreateToken(User user)
        {
            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("firstName", user.FirstName),
            new("lastName", user.LastName)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _opt.Issuer,
                audience: _opt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_opt.ExpiresMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
