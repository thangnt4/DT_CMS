using DT_CMS.Core.DTOs.LoaiTinTucs;
using DT_CMS.Core.DTOs.Common;

namespace DT_CMS.Core.Interfaces.Services;

public interface ILoaiTinTucService
{
    Task<PagedResultDto<LoaiTinTucDto>> GetLoaiTinTucsAsync(QueryParamsDto query);
    Task<LoaiTinTucDto> GetLoaiTinTucByIdAsync(int id);
    Task<LoaiTinTucDto> CreateLoaiTinTucAsync(CreateLoaiTinTucDto dto, int currentUserId);
    Task<LoaiTinTucDto> UpdateLoaiTinTucAsync(int id, UpdateLoaiTinTucDto dto, int currentUserId);
    Task DeleteLoaiTinTucAsync(int id);
}
