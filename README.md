# DT CMS

Full-stack starter: **.NET 10 Web API (Clean Architecture)** + **Angular (latest) + PrimeNG (Sakai layout)**.
Module mẫu CRUD hoàn chỉnh: **Người dùng (Users)**.

## 1. Cấu trúc thư mục

```
DT_Cms/
├─ DT_Api/
│  ├─ DT_Cms.sln
│  ├─ DT.Cms.Domain/          # Entities thuần, không phụ thuộc (Core)
│  ├─ DT.Cms.Application/     # DTOs, interfaces, business logic (CQRS-lite) (Core)
│  ├─ DT.Cms.Infrastructure/  # EF Core, Dapper, JWT, PasswordHasher
│  ├─ DT.Cms.Api/          # Controllers, Program.cs, DI composition root
│  └─ Database/
│     └─ Scripts/
│        ├─ 01_CreateDatabase_And_Tables.sql
│        └─ 02_StoredProcedures.sql
└─ DT_Web/                    # Angular + PrimeNG (Sakai layout)
```

**Nguyên tắc CQRS áp dụng trong module Users:**
- **Ghi** (Create/Update/Delete) → `UserCommandService` (`Application/Users/Services`) dùng **EF Core + LINQ**, cấu hình bảng bằng **Fluent API** (`Infrastructure/Persistence/Configurations/UserConfiguration.cs`).
- **Đọc / danh sách** → `UserQueryService` (`Infrastructure/Users/UserQueryService.cs`) gọi **Stored Procedure `sp_GetUsers_Search`** qua **Dapper**.

> Ghi chú kiến trúc: dự án **không dùng MediatR/AutoMapper/FluentValidation** (các thư viện này đã chuyển sang giấy phép thương mại từ 2025) để giữ project 100% miễn phí, dễ build ngay. CQRS được hiện thực bằng service đơn giản (`IUserCommandService` / `IUserQueryService`), mapping bằng extension method thủ công, validation bằng `DataAnnotations`.

## 2. Backend — chạy thử

### Yêu cầu
- .NET 10 SDK
- SQL Server (LocalDB / Express / Developer)

### Bước 1 — Tạo database
Mở SSMS / Azure Data Studio, chạy lần lượt:
```
DT_Api/Database/Scripts/01_CreateDatabase_And_Tables.sql
DT_Api/Database/Scripts/02_StoredProcedures.sql
```
(Hoặc dùng EF Core Migrations: từ thư mục `DT_Api/` chạy `dotnet ef migrations add InitialCreate -p DT.Cms.Infrastructure -s DT.Cms.Api` rồi `dotnet ef database update -p DT.Cms.Infrastructure -s DT.Cms.Api` — schema sinh ra khớp với Fluent API đã cấu hình.)

### Bước 2 — Cấu hình connection string & JWT
Sửa `DT_Api/DT.Cms.Api/appsettings.json` (hoặc dùng `dotnet user-secrets` cho môi trường dev):
```json
"ConnectionStrings": { "DefaultConnection": "Server=.;Database=DT_Cms;Trusted_Connection=True;TrustServerCertificate=True" },
"Jwt": { "Key": "..." }
```

### Bước 3 — Chạy API
```bash
cd DT_Api/DT.Cms.Api
dotnet restore
dotnet run
```
API chạy tại `http://localhost:5000`, Swagger tại `http://localhost:5000/swagger`.

Lần chạy đầu tiên, ứng dụng tự seed tài khoản quản trị:
```
Username: admin
Password: Admin@123
```

### Các endpoint chính
| Method | Endpoint              | Mô tả                                  | Auth |
|--------|------------------------|-----------------------------------------|------|
| POST   | `/api/auth/login`      | Đăng nhập, trả về JWT                   | ❌ |
| POST   | `/api/auth/logout`     | Logout phía client                      | ❌ |
| GET    | `/api/users`            | Danh sách (paging + keyword) qua SP    | ✅ |
| GET    | `/api/users/{id}`       | Chi tiết 1 user                        | ✅ |
| POST   | `/api/users`            | Tạo user (EF Core)                     | ✅ |
| PUT    | `/api/users/{id}`       | Cập nhật user (EF Core)                | ✅ |
| DELETE | `/api/users/{id}`       | Xóa user (EF Core)                     | ✅ |

## 3. Frontend — chạy thử

### Yêu cầu
- Node.js 20+
- Angular CLI (`npm i -g @angular/cli`)

### Cài đặt & chạy
```bash
cd DT_Web
npm install
npm start
```
App chạy tại `http://localhost:4200`, tự động gọi API tại `http://localhost:5000/api` (xem `src/environments/environment.ts`).

### Cấu trúc UI
- `/login` — màn hình đăng nhập.
- `/dashboard` — sau khi đăng nhập, layout chính (Sakai-style: Topbar + Sidebar).
- `/system/users` — **Hệ thống → Người dùng**: `p-table` (phân trang server-side, tìm kiếm), `p-dialog` (Thêm/Sửa), `p-confirmDialog` (xác nhận xóa).

## 4. Luồng kiểm thử nhanh
1. Chạy API + chạy Web song song.
2. Vào `http://localhost:4200`, đăng nhập `admin` / `Admin@123`.
3. Vào menu **Hệ thống → Người dùng**.
4. Thử **Thêm mới**, **Sửa**, **Xóa** (có confirm), và gõ vào ô tìm kiếm để test SP `sp_GetUsers_Search` với phân trang.

## 5. Lưu ý khi build thật (production-ready checklist)
- Đổi `Jwt:Key` sang giá trị bí mật thật, lưu qua `dotnet user-secrets` / biến môi trường / Key Vault — không commit secret thật vào appsettings.json.
- Bật HTTPS bắt buộc, cấu hình lại CORS `AllowedOrigins` theo domain thật khi deploy.
- Thêm refresh-token nếu cần phiên đăng nhập dài hơn `Jwt:ExpiryMinutes`.
- Thêm rate-limiting cho `/api/auth/login` để chống brute-force.
