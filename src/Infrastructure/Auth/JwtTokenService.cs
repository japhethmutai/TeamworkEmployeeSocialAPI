using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TeamworkApp.Application.Auth;
using TeamworkApp.Domain.Entities;

namespace TeamworkApp.Infrastructure.Auth;

public class JwtTokenService : IJwtTokenService
{
    public readonly JwtSettings _jwtSettings;
    public JwtTokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public TokenResult GenerateToken(User user)
    {
        // Claims = statements about the user that end up embedded (base64, not encrypted)
        // inside the token. Anything sensitive should NOT go here - it's readable by
        // anyone who has the token, just not forgeable without the signing key.
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Role, user.Role.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var expiresAtUtc = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

        // This signing key is just your secret string turned into bytes. Whoever holds
        // this key can both issue valid token AND verify them - that's what "symmetric"
        // means here, as opposed to a public/private keypair (RS256).
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expiresAtUtc,
            signingCredentials: credentials
        );

        // WriteToken serializes the JwtSecurityToken object into the actual
        // header.payload.signature string format you'd see on jwt.io.
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new TokenResult(tokenString, expiresAtUtc);
    }
}
