namespace DT_CMS.Core.DTOs.ChucVus;

public class ChucVuDto
{
    public int Id { get; set; }
    public string TenDuLieu { get; set; } = string.Empty;
    public string? GhiChu { get; set; }
    public int? TrangThai { get; set; }
    public DateTime NgayTao { get; set; }
    public DateTime? NgayCapNhat { get; set; }
}

public class CreateChucVuDto
{
    public string TenDuLieu { get; set; } = string.Empty;
    public string? GhiChu { get; set; }
    public int TrangThai { get; set; } = 1;
}

public class UpdateChucVuDto
{
    public string TenDuLieu { get; set; } = string.Empty;
    public string? GhiChu { get; set; }
    public int TrangThai { get; set; }
}
