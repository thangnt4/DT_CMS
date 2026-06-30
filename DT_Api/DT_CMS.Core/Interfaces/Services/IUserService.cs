using DT_CMS.Core.DTOs.Common;
using DT_CMS.Core.DTOs.Users;

namespace DT_CMS.Core.Interfaces.Services;

public interface IUserService
{
    Task<PagedResultDto<UserDto>> GetUsersAsync(QueryParamsDto query);
    Task<UserDto> GetUserByIdAsync(int id);
    Task<UserDto> CreateUserAsync(CreateUserDto dto);
    Task<UserDto> UpdateUserAsync(int id, UpdateUserDto dto);
    Task DeleteUserAsync(int id);
}
