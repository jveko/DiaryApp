using System.Security.Claims;

namespace DiaryApp.Interfaces;

public interface ITokenService
{
    public string GenerateAccessToken(int id);
    public string GenerateRefreshToken(int id);
    public ClaimsPrincipal? ValidateRefreshToken(string refreshToken);
    public ClaimsPrincipal? ValidateAccessToken(string accessToken);
}