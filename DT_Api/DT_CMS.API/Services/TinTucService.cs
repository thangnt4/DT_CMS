using DT_CMS.Core.DTOs.TinTucs;
using DT_CMS.Core.DTOs.Common;
using DT_CMS.Core.Entities;
using DT_CMS.Core.Exceptions;
using DT_CMS.Core.Interfaces.Services;
using DT_CMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DT_CMS.API.Services;

public class TinTucService : ITinTucService
{
    private readonly ApplicationDbContext _db;

    public TinTucService(ApplicationDbContext db) => _db = db;

    public async Task<PagedResultDto<TinTucDto>> GetTinTucsAsync(QueryParamsDto query)
    {
        var queryable = _db.TinTucs.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
            queryable = queryable.Where(c => c.TenDuLieu.Contains(query.Search));

        var totalCount = await queryable.CountAsync();

        var entities = await queryable
            .OrderByDescending(c => c.NgayTao)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new PagedResultDto<TinTucDto>
        {
            Items = entities.Select(ToDto).ToList(),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<TinTucDto> GetTinTucByIdAsync(int id)
    {
        var TinTuc = await _db.TinTucs.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new NotFoundException("TinTuc", id);
        return ToDto(TinTuc);
    }

    public async Task<TinTucDto> CreateTinTucAsync(CreateTinTucDto dto, int currentUserId)
    {
        if (await _db.TinTucs.AsNoTracking().AnyAsync(c => c.TenDuLieu == dto.TenDuLieu))
            throw new AppException($"Tin tức '{dto.TenDuLieu}' đã tồn tại.");

        var TinTuc = new TinTuc
        {
            TenDuLieu = dto.TenDuLieu,
            GhiChu = dto.GhiChu,
            TrangThai = dto.TrangThai,
            NguoiTao = currentUserId,
            NgayTao = DateTime.Now
        };

        _db.TinTucs.Add(TinTuc);
        await _db.SaveChangesAsync();

        return ToDto(TinTuc);
    }

    public async Task<TinTucDto> UpdateTinTucAsync(int id, UpdateTinTucDto dto, int currentUserId)
    {
        var TinTuc = await _db.TinTucs.FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new NotFoundException("TinTuc", id);

        if (await _db.TinTucs.AsNoTracking().AnyAsync(c => c.Id != id && c.TenDuLieu == dto.TenDuLieu))
            throw new AppException($"Tin tức '{dto.TenDuLieu}' đã tồn tại.");

        TinTuc.TenDuLieu = dto.TenDuLieu;
        TinTuc.GhiChu = dto.GhiChu;
        TinTuc.TrangThai = dto.TrangThai;
        TinTuc.NguoiCapNhat = currentUserId;
        TinTuc.NgayCapNhat = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return ToDto(TinTuc);
    }

    public async Task DeleteTinTucAsync(int id)
    {
        var TinTuc = await _db.TinTucs.FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new NotFoundException("TinTuc", id);

        TinTuc.Xoa = true;
        await _db.SaveChangesAsync();
    }

    private static TinTucDto ToDto(TinTuc TinTuc) => new()
    {
        Id = TinTuc.Id,
        TenDuLieu = TinTuc.TenDuLieu,
        GhiChu = TinTuc.GhiChu,
        TrangThai = TinTuc.TrangThai,
        NgayTao = TinTuc.NgayTao,
        NgayCapNhat = TinTuc.NgayCapNhat
    };
}
