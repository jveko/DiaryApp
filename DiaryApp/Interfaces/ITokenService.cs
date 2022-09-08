using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace DiaryApp.Interfaces;

public interface ITokenService
{
    public string GenerateAccessToken(int id);
    public string GenerateRefreshToken(int id);
    public ClaimsPrincipal? ValidateRefreshToken(string refreshToken);
    public ClaimsPrincipal? ValidateAccessToken(string accessToken);
}