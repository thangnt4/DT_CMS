using DT_CMS.Core.DTOs.SanPhams;
using DT_CMS.Core.DTOs.Common;

namespace DT_CMS.Core.Interfaces.Services;

public interface ISanPhamService
{
    Task<PagedResultDto<SanPhamDto>> GetSanPhamsAsync(QueryParamsDto query);
    Task<SanPhamDto> GetSanPhamByIdAsync(int id);
    Task<SanPhamDto> CreateSanPhamAsync(CreateSanPhamDto dto, int currentUserId);
    Task<SanPhamDto> UpdateSanPhamAsync(int id, UpdateSanPhamDto dto, int currentUserId);
    Task DeleteSanPhamAsync(int id);
}
