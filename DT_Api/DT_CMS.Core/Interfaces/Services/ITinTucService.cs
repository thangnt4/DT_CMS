using DT_CMS.Core.DTOs.TinTucs;
using DT_CMS.Core.DTOs.Common;

namespace DT_CMS.Core.Interfaces.Services;

public interface ITinTucService
{
    Task<PagedResultDto<TinTucDto>> GetTinTucsAsync(QueryParamsDto query);
    Task<TinTucDto> GetTinTucByIdAsync(int id);
    Task<TinTucDto> CreateTinTucAsync(CreateTinTucDto dto, int currentUserId);
    Task<TinTucDto> UpdateTinTucAsync(int id, UpdateTinTucDto dto, int currentUserId);
    Task DeleteTinTucAsync(int id);
}
