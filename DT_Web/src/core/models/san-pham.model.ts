export interface SanPham {
  id: number;
  tenDuLieu: string;
  ghiChu?: string | null;
  trangThai: number;
  fileDinhKem?: string | null;
  ngayTao: string;
  ngayCapNhat?: string | null;
}

export interface CreateSanPham {
  tenDuLieu: string;
  ghiChu?: string | null;
  trangThai: number;
}

export interface UpdateSanPham {
  tenDuLieu: string;
  ghiChu?: string | null;
  trangThai: number;
}
