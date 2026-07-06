using DT_CMS.Core.DTOs.Common;
using DT_CMS.Core.DTOs.SanPhams;
using DT_CMS.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DT_CMS.API.Controllers;

[ApiController]
[Route("api/sanpham")]
[Authorize]
public class SanPhamController : ControllerBase
{
    private readonly ISanPhamService _sanPhamService;

    public SanPhamController(ISanPhamService sanPhamService)
    {
        _sanPhamService = sanPhamService;
    }

    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParamsDto query)
    {
        var result = await _sanPhamService.GetSanPhamsAsync(query);
        return Ok(ApiResponseDto<PagedResultDto<SanPhamDto>>.Ok(result));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _sanPhamService.GetSanPhamByIdAsync(id);
        return Ok(ApiResponseDto<SanPhamDto>.Ok(result));
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] CreateSanPhamDto dto)
    {
        var result = await _sanPhamService.CreateSanPhamAsync(dto, CurrentUserId);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            ApiResponseDto<SanPhamDto>.Ok(result, "Tạo sản phẩm thành công."));
    }

    [HttpPut("{id:int}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Update(int id, [FromForm] UpdateSanPhamDto dto)
    {
        var result = await _sanPhamService.UpdateSanPhamAsync(id, dto, CurrentUserId);

        return Ok(ApiResponseDto<SanPhamDto>.Ok(result, "Cập nhật sản phẩm thành công."));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _sanPhamService.DeleteSanPhamAsync(id);
        return Ok(ApiResponseDto.Ok("Xóa sản phẩm thành công."));
    }
}