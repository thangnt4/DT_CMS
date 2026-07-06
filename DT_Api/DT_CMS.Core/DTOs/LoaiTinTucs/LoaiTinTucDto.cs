namespace DT_CMS.Core.DTOs.LoaiTinTucs;

public class LoaiTinTucDto
{
    public int Id { get; set; }
    public string TenDuLieu { get; set; } = string.Empty;
    public string? GhiChu { get; set; }
    public int? TrangThai { get; set; }
    public DateTime NgayTao { get; set; }
    public DateTime? NgayCapNhat { get; set; }
}

public class CreateLoaiTinTucDto
{
    public string TenDuLieu { get; set; } = string.Empty;
    public string? GhiChu { get; set; }
    public int TrangThai { get; set; } = 1;
}

public class UpdateLoaiTinTucDto
{
    public string TenDuLieu { get; set; } = string.Empty;
    public string? GhiChu { get; set; }
    public int TrangThai { get; set; }
}
