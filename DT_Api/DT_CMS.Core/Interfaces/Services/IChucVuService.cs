using DT_CMS.Core.DTOs.ChucVus;
using DT_CMS.Core.DTOs.Common;

namespace DT_CMS.Core.Interfaces.Services;

public interface IChucVuService
{
    Task<PagedResultDto<ChucVuDto>> GetChucVusAsync(QueryParamsDto query);
    Task<ChucVuDto> GetChucVuByIdAsync(int id);
    Task<ChucVuDto> CreateChucVuAsync(CreateChucVuDto dto, int currentUserId);
    Task<ChucVuDto> UpdateChucVuAsync(int id, UpdateChucVuDto dto, int currentUserId);
    Task DeleteChucVuAsync(int id);
}
