using TeamworkApp.Domain.Entities;

namespace TeamworkApp.Application.Auth;

public record TokenResult(string Token, DateTime ExpiresAtUtc);
public interface IJwtTokenService
{
    TokenResult GenerateToken(User user);
}
