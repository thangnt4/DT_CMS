using DT_CMS.Core.DTOs.LoaiTinTucs;
using DT_CMS.Core.DTOs.Common;
using DT_CMS.Core.Entities;
using DT_CMS.Core.Exceptions;
using DT_CMS.Core.Interfaces.Services;
using DT_CMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DT_CMS.API.Services;

public class LoaiTinTucService : ILoaiTinTucService
{
    private readonly ApplicationDbContext _db;

    public LoaiTinTucService(ApplicationDbContext db) => _db = db;

    public async Task<PagedResultDto<LoaiTinTucDto>> GetLoaiTinTucsAsync(QueryParamsDto query)
    {
        var queryable = _db.LoaiTinTucs.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
            queryable = queryable.Where(c => c.TenDuLieu.Contains(query.Search));

        var totalCount = await queryable.CountAsync();

        var entities = await queryable
            .OrderByDescending(c => c.NgayTao)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new PagedResultDto<LoaiTinTucDto>
        {
            Items = entities.Select(ToDto).ToList(),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<LoaiTinTucDto> GetLoaiTinTucByIdAsync(int id)
    {
        var LoaiTinTuc = await _db.LoaiTinTucs.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new NotFoundException("LoaiTinTuc", id);
        return ToDto(LoaiTinTuc);
    }

    public async Task<LoaiTinTucDto> CreateLoaiTinTucAsync(CreateLoaiTinTucDto dto, int currentUserId)
    {
        if (await _db.LoaiTinTucs.AsNoTracking().AnyAsync(c => c.TenDuLieu == dto.TenDuLieu))
            throw new AppException($"Loại tin tức '{dto.TenDuLieu}' đã tồn tại.");

        var LoaiTinTuc = new LoaiTinTuc
        {
            TenDuLieu = dto.TenDuLieu,
            GhiChu = dto.GhiChu,
            TrangThai = dto.TrangThai,
            NguoiTao = currentUserId,
            NgayTao = DateTime.Now
        };

        _db.LoaiTinTucs.Add(LoaiTinTuc);
        await _db.SaveChangesAsync();

        return ToDto(LoaiTinTuc);
    }

    public async Task<LoaiTinTucDto> UpdateLoaiTinTucAsync(int id, UpdateLoaiTinTucDto dto, int currentUserId)
    {
        var LoaiTinTuc = await _db.LoaiTinTucs.FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new NotFoundException("LoaiTinTuc", id);

        if (await _db.LoaiTinTucs.AsNoTracking().AnyAsync(c => c.Id != id && c.TenDuLieu == dto.TenDuLieu))
            throw new AppException($"Loại tin tức '{dto.TenDuLieu}' đã tồn tại.");

        LoaiTinTuc.TenDuLieu = dto.TenDuLieu;
        LoaiTinTuc.GhiChu = dto.GhiChu;
        LoaiTinTuc.TrangThai = dto.TrangThai;
        LoaiTinTuc.NguoiCapNhat = currentUserId;
        LoaiTinTuc.NgayCapNhat = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return ToDto(LoaiTinTuc);
    }

    public async Task DeleteLoaiTinTucAsync(int id)
    {
        var LoaiTinTuc = await _db.LoaiTinTucs.FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new NotFoundException("LoaiTinTuc", id);

        LoaiTinTuc.Xoa = true;
        await _db.SaveChangesAsync();
    }

    private static LoaiTinTucDto ToDto(LoaiTinTuc LoaiTinTuc) => new()
    {
        Id = LoaiTinTuc.Id,
        TenDuLieu = LoaiTinTuc.TenDuLieu,
        GhiChu = LoaiTinTuc.GhiChu,
        TrangThai = LoaiTinTuc.TrangThai,
        NgayTao = LoaiTinTuc.NgayTao,
        NgayCapNhat = LoaiTinTuc.NgayCapNhat
    };
}
