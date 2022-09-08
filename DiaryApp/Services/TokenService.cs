using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DiaryApp.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace DiaryApp.Services;

public class TokenService : ITokenService
{
    private const string AccessTokenSecret = "RGlhcnlBcHBBY2Nlc3NUb2tlbg==";
    private const string RefreshTokenSecret = "RGlhcnlBcHBSZWZyZXNoVG9rZW4=";
    private const string SecurityAlgorithm = SecurityAlgorithms.HmacSha256;

    public string GenerateAccessToken(int id)
    {
        var key = Convert.FromBase64String(AccessTokenSecret);
        var securityKey = new SymmetricSecurityKey(key);
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("userId", id.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithm)
        };
        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateJwtSecurityToken(descriptor);
        return handler.WriteToken(token);
    }

    public string GenerateRefreshToken(int id)
    {
        var key = Convert.FromBase64String(RefreshTokenSecret);
        var securityKey = new SymmetricSecurityKey(key);
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("userId", id.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithm)
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateJwtSecurityToken(descriptor);
        return handler.WriteToken(token);
    }

    public ClaimsPrincipal? ValidateRefreshToken(string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = tokenHandler.ValidateToken(refreshToken, GetParamRefreshToken(), out var validatedToken);
        return validatedToken.ValidTo > DateTime.UtcNow ? claims : null;
    }

    public ClaimsPrincipal? ValidateAccessToken(string accessToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = tokenHandler.ValidateToken(accessToken, GetParamAccessToken(), out var validatedToken);
        return validatedToken.ValidTo > DateTime.UtcNow ? claims : null;
    }

    private static TokenValidationParameters GetParamRefreshToken()
    {
        return new TokenValidationParameters
        {
            ValidateLifetime = false,
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(RefreshTokenSecret))
        };
    }

    private static TokenValidationParameters GetParamAccessToken()
    {
        return new TokenValidationParameters
        {
            ValidateLifetime = false,
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(AccessTokenSecret))
        };
    }
}