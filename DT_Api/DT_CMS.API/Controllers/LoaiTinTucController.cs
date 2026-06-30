using System.Security.Claims;
using DT_CMS.Core.DTOs.LoaiTinTucs;
using DT_CMS.Core.DTOs.Common;
using DT_CMS.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DT_CMS.API.Controllers;

[ApiController]
[Route("api/loaitintuc")]
[Authorize]
public class LoaiTinTucController : ControllerBase
{
    private readonly ILoaiTinTucService _loaiTinTucService;

    public LoaiTinTucController(ILoaiTinTucService loaiTinTucService) => _loaiTinTucService = loaiTinTucService;

    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParamsDto query)
    {
        var result = await _loaiTinTucService.GetLoaiTinTucsAsync(query);
        return Ok(ApiResponseDto<PagedResultDto<LoaiTinTucDto>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _loaiTinTucService.GetLoaiTinTucByIdAsync(id);
        return Ok(ApiResponseDto<LoaiTinTucDto>.Ok(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLoaiTinTucDto dto)
    {
        var result = await _loaiTinTucService.CreateLoaiTinTucAsync(dto, CurrentUserId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponseDto<LoaiTinTucDto>.Ok(result, "Tạo loại tin tức thành công."));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLoaiTinTucDto dto)
    {
        var result = await _loaiTinTucService.UpdateLoaiTinTucAsync(id, dto, CurrentUserId);
        return Ok(ApiResponseDto<LoaiTinTucDto>.Ok(result, "Cập nhật thành công."));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _loaiTinTucService.DeleteLoaiTinTucAsync(id);
        return Ok(ApiResponseDto.Ok("Xóa loại tin tức thành công."));
    }
}
