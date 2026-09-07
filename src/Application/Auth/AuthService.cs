
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TeamworkApp.Application.Persistence;
using TeamworkApp.Domain.Entities;

namespace TeamworkApp.Application.Auth;

public class AuthService : IAuthService
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtTokenService _tokenService;
    private readonly IPasswordHasher<User> _passwordHasher;
    public AuthService(
        IApplicationDbContext context,
        IJwtTokenService tokenService,
        IPasswordHasher<User> passwordHasher
    )
    {
        _context = context;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null)
        {
            throw new InvalidCredentialsException();
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            throw new InvalidCredentialsException();
        }

        var token = _tokenService.GenerateToken(user);

        return new AuthResult(user.Id, user.Email, user.Role.ToString(), token.Token, token.ExpiresAtUtc);
    }

    public async Task<AuthResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var emailInUse = await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);

        if (emailInUse)
        {
            throw new EmailAlreadyInUseException(request.Email);
        }

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Role = UserRole.Employee
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        var token = _tokenService.GenerateToken(user);

        return new AuthResult(user.Id, user.Email, user.Role.ToString(), token.Token, token.ExpiresAtUtc);
    }
}
