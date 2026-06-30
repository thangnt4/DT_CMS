export interface ChucVu {
  id: number;
  tenDuLieu: string;
  ghiChu?: string | null;
  trangThai: number;
  ngayTao: string;
  ngayCapNhat?: string | null;
}

export interface CreateChucVu {
  tenDuLieu: string;
  ghiChu?: string | null;
  trangThai: number;
}

export interface UpdateChucVu {
  tenDuLieu: string;
  ghiChu?: string | null;
  trangThai: number;
}
