using System.Security.Claims;
using DT_CMS.Core.DTOs.TinTucs;
using DT_CMS.Core.DTOs.Common;
using DT_CMS.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DT_CMS.API.Controllers;

[ApiController]
[Route("api/tintuc")]
[Authorize]
public class TinTucController : ControllerBase
{
    private readonly ITinTucService _tinTucService;

    public TinTucController(ITinTucService tinTucService) => _tinTucService = tinTucService;

    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParamsDto query)
    {
        var result = await _tinTucService.GetTinTucsAsync(query);
        return Ok(ApiResponseDto<PagedResultDto<TinTucDto>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _tinTucService.GetTinTucByIdAsync(id);
        return Ok(ApiResponseDto<TinTucDto>.Ok(result));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTinTucDto dto)
    {
        var result = await _tinTucService.CreateTinTucAsync(dto, CurrentUserId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponseDto<TinTucDto>.Ok(result, "Tạo tin tức thành công."));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTinTucDto dto)
    {
        var result = await _tinTucService.UpdateTinTucAsync(id, dto, CurrentUserId);
        return Ok(ApiResponseDto<TinTucDto>.Ok(result, "Cập nhật thành công."));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _tinTucService.DeleteTinTucAsync(id);
        return Ok(ApiResponseDto.Ok("Xóa tin tức thành công."));
    }
}
