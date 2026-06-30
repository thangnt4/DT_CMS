using DT_CMS.Core.DTOs.Common;
using DT_CMS.Core.DTOs.Users;
using DT_CMS.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DT_CMS.API.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParamsDto query)
    {
        var result = await _userService.GetUsersAsync(query);
        return Ok(ApiResponseDto<PagedResultDto<UserDto>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _userService.GetUserByIdAsync(id);
        return Ok(ApiResponseDto<UserDto>.Ok(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        var result = await _userService.CreateUserAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponseDto<UserDto>.Ok(result, "Tạo người dùng thành công."));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
    {
        var result = await _userService.UpdateUserAsync(id, dto);
        return Ok(ApiResponseDto<UserDto>.Ok(result, "Cập nhật thành công."));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _userService.DeleteUserAsync(id);
        return Ok(ApiResponseDto.Ok("Xóa người dùng thành công."));
    }
}
