using DT_CMS.Core.DTOs.Auth;
using DT_CMS.Core.DTOs.Common;
using DT_CMS.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DT_CMS.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);
        return Ok(ApiResponseDto<LoginResponseDto>.Ok(result, "Đăng nhập thành công."));
    }

    /// <summary>
    /// Stateless JWT has no server-side session to invalidate; the client
    /// discards the token. This endpoint exists so the front-end has a
    /// single, consistent place to call on sign-out.
    /// </summary>
    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout() => Ok(ApiResponseDto.Ok("Đăng xuất thành công."));
}
