using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace DiaryApp.Utilities;

public class TokenManager
{
    private const string AccessTokenSecret = "RGlhcnlBcHBBY2Nlc3NUb2tlbg==";
    private const string RefreshTokenSecret = "RGlhcnlBcHBSZWZyZXNoVG9rZW4=";
    private const string SecurityAlgorithm = SecurityAlgorithms.HmacSha256;

    public static string GenerateAccessToken(int id)
    {
        var key = Convert.FromBase64String(AccessTokenSecret);
        var securityKey = new SymmetricSecurityKey(key);
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("userId", id.ToString())
            }),
            Expires = DateTime.UtcNow.AddMilliseconds(10),
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithm)
        };
        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateJwtSecurityToken(descriptor);
        return handler.WriteToken(token);
    }

    public static string GenerateRefreshToken(int id)
    {
        var key = Convert.FromBase64String(RefreshTokenSecret);
        var securityKey = new SymmetricSecurityKey(key);
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("userId", id.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(3),
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithm)
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateJwtSecurityToken(descriptor);
        return handler.WriteToken(token);
    }

    public static ClaimsPrincipal? ValidateRefreshToken(string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.ValidateToken(refreshToken, GetValidationParametersRefreshToken(), out var validatedToken);
    }
    public static ClaimsPrincipal? ValidateAccessToken(string accessToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.ValidateToken(accessToken, GetValidationParametersAccessToken(), out var validatedToken);
    }

    private static TokenValidationParameters GetValidationParametersRefreshToken()
    {
        return new TokenValidationParameters()
        {
            ValidateLifetime = false,
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Convert.FromBase64String(RefreshTokenSecret)) // The same key as the one that generate the token
        };
    }
    private static TokenValidationParameters GetValidationParametersAccessToken()
    {
        return new TokenValidationParameters()
        {
            ValidateLifetime = false,
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Convert.FromBase64String(AccessTokenSecret)) // The same key as the one that generate the token
        };
    }
}