using DT_CMS.Core.DTOs.Auth;

namespace DT_CMS.Core.Interfaces.Services;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
}
