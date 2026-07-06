-- Thêm cột FileDinhKem cho bảng Dm_SanPham (lưu đường dẫn tương đối tới file đính kèm)
ALTER TABLE Dm_SanPham ADD FileDinhKem NVARCHAR(500) NULL;
