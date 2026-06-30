using DT_CMS.API.Authorization;
using DT_CMS.Core.DTOs.Auth;
using DT_CMS.Core.Exceptions;
using DT_CMS.Core.Interfaces.Services;
using DT_CMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DT_CMS.API.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _db;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        ApplicationDbContext db,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IOptions<JwtSettings> jwtSettings)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null || !_passwordHasher.Verify(user.PasswordHash, request.Password))
            throw new UnauthorizedException("Tài khoản hoặc mật khẩu không đúng.");

        if (!user.IsActive)
            throw new UnauthorizedException("Tài khoản đã bị khóa.");

        var accessToken = _tokenService.GenerateAccessToken(user);

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            ExpiresIn = _jwtSettings.AccessTokenExpirationMinutes * 60,
            User = new UserInfoDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName
            }
        };
    }
}
