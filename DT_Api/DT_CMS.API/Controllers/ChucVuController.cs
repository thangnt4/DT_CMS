using System.Security.Claims;
using DT_CMS.Core.DTOs.ChucVus;
using DT_CMS.Core.DTOs.Common;
using DT_CMS.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DT_CMS.API.Controllers;

[ApiController]
[Route("api/chucvu")]
[Authorize]
public class ChucVuController : ControllerBase
{
    private readonly IChucVuService _chucVuService;

    public ChucVuController(IChucVuService chucVuService) => _chucVuService = chucVuService;

    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParamsDto query)
    {
        var result = await _chucVuService.GetChucVusAsync(query);
        return Ok(ApiResponseDto<PagedResultDto<ChucVuDto>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _chucVuService.GetChucVuByIdAsync(id);
        return Ok(ApiResponseDto<ChucVuDto>.Ok(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateChucVuDto dto)
    {
        var result = await _chucVuService.CreateChucVuAsync(dto, CurrentUserId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponseDto<ChucVuDto>.Ok(result, "Tạo chức vụ thành công."));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateChucVuDto dto)
    {
        var result = await _chucVuService.UpdateChucVuAsync(id, dto, CurrentUserId);
        return Ok(ApiResponseDto<ChucVuDto>.Ok(result, "Cập nhật thành công."));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _chucVuService.DeleteChucVuAsync(id);
        return Ok(ApiResponseDto.Ok("Xóa chức vụ thành công."));
    }
}
