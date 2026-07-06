namespace DT_CMS.Core.Entities;

public class SanPham
{
    public int Id { get; set; }
    public string TenDuLieu { get; set; } = string.Empty;
    public string? GhiChu { get; set; }
    public string? FileDinhKem { get; set; }
    public int? TrangThai { get; set; } = 1;
    public int? NguoiTao { get; set; }
    public DateTime NgayTao { get; set; } = DateTime.Now;
    public int? NguoiCapNhat { get; set; }
    public DateTime? NgayCapNhat { get; set; }
    public bool? Xoa { get; set; }
}
