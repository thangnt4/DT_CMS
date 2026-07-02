export interface TinTuc {
  id: number;
  tenDuLieu: string;
  ghiChu?: string | null;
  trangThai: number;
  nguoiTao: string;
  ngayTao: string;
  nguoiCapNhat?: string | null;
  ngayCapNhat?: string | null;
  xoa?: boolean | null;
}

export interface CreateTinTuc {
  tenDuLieu: string;
  ghiChu?: string | null;
  trangThai: number;
}

export interface UpdateTinTuc {
  tenDuLieu: string;
  ghiChu?: string | null;
  trangThai: number;
}
