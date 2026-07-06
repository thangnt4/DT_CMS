using DT_CMS.Core.DTOs.SanPhams;
using DT_CMS.Core.DTOs.Common;
using DT_CMS.Core.Entities;
using DT_CMS.Core.Exceptions;
using DT_CMS.Core.Interfaces.Services;
using DT_CMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DT_CMS.API.Services;

public class SanPhamService : ISanPhamService
{
    private readonly ApplicationDbContext _db;

    public SanPhamService(ApplicationDbContext db) => _db = db;

    public async Task<PagedResultDto<SanPhamDto>> GetSanPhamsAsync(QueryParamsDto query)
    {
        var queryable = _db.SanPhams.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
            queryable = queryable.Where(c => c.TenDuLieu.Contains(query.Search));

        var totalCount = await queryable.CountAsync();

        var entities = await queryable
            .OrderByDescending(c => c.NgayTao)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new PagedResultDto<SanPhamDto>
        {
            Items = entities.Select(ToDto).ToList(),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<SanPhamDto> GetSanPhamByIdAsync(int id)
    {
        var SanPham = await _db.SanPhams.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new NotFoundException("SanPham", id);
        return ToDto(SanPham);
    }

    public async Task<SanPhamDto> CreateSanPhamAsync(CreateSanPhamDto dto, int currentUserId)
    {
        if (await _db.SanPhams.AsNoTracking().AnyAsync(c => c.TenDuLieu == dto.TenDuLieu))
            throw new AppException($"Chức vụ '{dto.TenDuLieu}' đã tồn tại.");

        var SanPham = new SanPham
        {
            TenDuLieu = dto.TenDuLieu,
            GhiChu = dto.GhiChu,
            TrangThai = dto.TrangThai,
            NguoiTao = currentUserId,
            NgayTao = DateTime.Now
        };

        _db.SanPhams.Add(SanPham);
        await _db.SaveChangesAsync();

        return ToDto(SanPham);
    }

    public async Task<SanPhamDto> UpdateSanPhamAsync(int id, UpdateSanPhamDto dto, int currentUserId)
    {
        var SanPham = await _db.SanPhams.FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new NotFoundException("SanPham", id);

        if (await _db.SanPhams.AsNoTracking().AnyAsync(c => c.Id != id && c.TenDuLieu == dto.TenDuLieu))
            throw new AppException($"Chức vụ '{dto.TenDuLieu}' đã tồn tại.");

        SanPham.TenDuLieu = dto.TenDuLieu;
        SanPham.GhiChu = dto.GhiChu;
        SanPham.TrangThai = dto.TrangThai;
        SanPham.NguoiCapNhat = currentUserId;
        SanPham.NgayCapNhat = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return ToDto(SanPham);
    }

    public async Task DeleteSanPhamAsync(int id)
    {
        var SanPham = await _db.SanPhams.FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new NotFoundException("SanPham", id);

        SanPham.Xoa = true;
        await _db.SaveChangesAsync();
    }

    private static SanPhamDto ToDto(SanPham SanPham) => new()
    {
        Id = SanPham.Id,
        TenDuLieu = SanPham.TenDuLieu,
        GhiChu = SanPham.GhiChu,
        TrangThai = SanPham.TrangThai,
        NgayTao = SanPham.NgayTao,
        NgayCapNhat = SanPham.NgayCapNhat
    };
}
