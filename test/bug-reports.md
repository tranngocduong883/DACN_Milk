# Bug Report – NutriMilk

## BUG-01: Đăng ký bằng email đã tồn tại trả về lỗi 500 thay vì thông báo trùng email

| Trường | Nội dung |
|---|---|
| ID | BUG-01 |
| Chức năng | Đăng ký tài khoản, cập nhật thông tin cá nhân |
| API | `POST /api/Authentication/Register`, `PUT /api/TaiKhoan/profile/{id}` |
| Môi trường | Backend chạy local (.NET 8), SQL Server 2019, Postman / Newman |
| Severity | Major |
| Priority | High |
| Cách phát hiện | Chạy bộ test API tự động ([NutriMilk.postman_collection.json](api/NutriMilk.postman_collection.json)) lần thứ hai |
| Trạng thái | Closed (đã sửa và retest) |

### Các bước tái hiện

1. Đăng ký tài khoản A với email `a@example.com` → thành công.
2. Đăng ký tài khoản B (tên đăng nhập khác) nhưng dùng lại email `a@example.com`:

```json
POST /api/Authentication/Register
{ "tenDangNhap": "user_b", "matKhau": "123456", "email": "a@example.com" }
```

### Kết quả mong đợi

HTTP 400, thông báo rõ nguyên nhân:

```json
{ "success": false, "message": "Email đã được sử dụng", "data": null }
```

### Kết quả thực tế

HTTP 500, thông báo chung chung, người dùng không biết phải sửa gì:

```json
{ "success": false, "message": "Đã xảy ra lỗi hệ thống, vui lòng thử lại sau", "data": null }
```

### Nguyên nhân

Cột `email` của bảng `TAI_KHOAN` có ràng buộc `UNIQUE`. Service chỉ kiểm tra trùng tên đăng nhập trước khi lưu, không kiểm tra trùng email, nên database ném lỗi vi phạm ràng buộc và middleware trả về 500. Chức năng cập nhật thông tin cá nhân có cùng lỗi khi đổi sang email của tài khoản khác.

### Cách sửa

Kiểm tra email đã tồn tại trước khi lưu (`TaiKhoanService.DangKyAsync` và `UpdateProfile`), trả về 400 với thông báo "Email đã được sử dụng".

### Retest

| Trường hợp | Kết quả |
|---|---|
| Đăng ký trùng email | 400, "Email đã được sử dụng" – Pass |
| Đổi email sang email của tài khoản khác | 400, "Email đã được sử dụng" – Pass |
| Cập nhật hồ sơ, giữ nguyên email của mình | 200 – Pass |
| Chạy lại toàn bộ bộ test API (regression) | 28/28 assertion Pass |
