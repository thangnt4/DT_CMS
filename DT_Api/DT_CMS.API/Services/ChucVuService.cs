using DT_CMS.Core.DTOs.ChucVus;
using DT_CMS.Core.DTOs.Common;
using DT_CMS.Core.Entities;
using DT_CMS.Core.Exceptions;
using DT_CMS.Core.Interfaces.Services;
using DT_CMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DT_CMS.API.Services;

public class ChucVuService : IChucVuService
{
    private readonly ApplicationDbContext _db;

    public ChucVuService(ApplicationDbContext db) => _db = db;

    public async Task<PagedResultDto<ChucVuDto>> GetChucVusAsync(QueryParamsDto query)
    {
        var queryable = _db.ChucVus.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
            queryable = queryable.Where(c => c.TenDuLieu.Contains(query.Search));

        var totalCount = await queryable.CountAsync();

        var entities = await queryable
            .OrderByDescending(c => c.NgayTao)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new PagedResultDto<ChucVuDto>
        {
            Items = entities.Select(ToDto).ToList(),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<ChucVuDto> GetChucVuByIdAsync(int id)
    {
        var chucVu = await _db.ChucVus.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new NotFoundException("ChucVu", id);
        return ToDto(chucVu);
    }

    public async Task<ChucVuDto> CreateChucVuAsync(CreateChucVuDto dto, int currentUserId)
    {
        if (await _db.ChucVus.AsNoTracking().AnyAsync(c => c.TenDuLieu == dto.TenDuLieu))
            throw new AppException($"Chức vụ '{dto.TenDuLieu}' đã tồn tại.");

        var chucVu = new ChucVu
        {
            TenDuLieu = dto.TenDuLieu,
            GhiChu = dto.GhiChu,
            TrangThai = dto.TrangThai,
            NguoiTao = currentUserId,
            NgayTao = DateTime.Now
        };

        _db.ChucVus.Add(chucVu);
        await _db.SaveChangesAsync();

        return ToDto(chucVu);
    }

    public async Task<ChucVuDto> UpdateChucVuAsync(int id, UpdateChucVuDto dto, int currentUserId)
    {
        var chucVu = await _db.ChucVus.FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new NotFoundException("ChucVu", id);

        if (await _db.ChucVus.AsNoTracking().AnyAsync(c => c.Id != id && c.TenDuLieu == dto.TenDuLieu))
            throw new AppException($"Chức vụ '{dto.TenDuLieu}' đã tồn tại.");

        chucVu.TenDuLieu = dto.TenDuLieu;
        chucVu.GhiChu = dto.GhiChu;
        chucVu.TrangThai = dto.TrangThai;
        chucVu.NguoiCapNhat = currentUserId;
        chucVu.NgayCapNhat = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return ToDto(chucVu);
    }

    public async Task DeleteChucVuAsync(int id)
    {
        var chucVu = await _db.ChucVus.FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new NotFoundException("ChucVu", id);

        chucVu.Xoa = true;
        await _db.SaveChangesAsync();
    }

    private static ChucVuDto ToDto(ChucVu chucVu) => new()
    {
        Id = chucVu.Id,
        TenDuLieu = chucVu.TenDuLieu,
        GhiChu = chucVu.GhiChu,
        TrangThai = chucVu.TrangThai,
        NgayTao = chucVu.NgayTao,
        NgayCapNhat = chucVu.NgayCapNhat
    };
}
