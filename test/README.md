# Kiểm thử – NutriMilk

| File | Nội dung |
|---|---|
| [test-cases.md](test-cases.md) | 42 test case thủ công cho 10 chức năng (đọc trực tiếp trên GitHub) |
| [test-cases.csv](test-cases.csv) | Cùng nội dung, mở bằng Excel / Google Sheets |
| [test-report.md](test-report.md) | Báo cáo kết quả kiểm thử thủ công |
| [bug-reports.md](bug-reports.md) | Bug tìm được và kết quả retest |
| [api/NutriMilk.postman_collection.json](api/NutriMilk.postman_collection.json) | Bộ test API tự động (14 request, 28 assertion) |

## Kiểm thử thủ công

Test case được viết từ đặc tả use case trong [báo cáo đồ án](../docs/BaoCao_DoAn_NutriMilk.pdf), chạy trên Google Chrome và Microsoft Edge. Mỗi test case gồm ID, điều kiện cần, các bước thực hiện, kết quả mong đợi và kết quả thực tế.

## Kiểm thử API tự động

Bộ test Postman kiểm tra:

- **Xác thực:** đăng nhập sai, đăng ký với mật khẩu 5 ký tự (dưới biên) và 6 ký tự (đúng biên), email sai định dạng, trùng tên đăng nhập, trùng email.
- **Sản phẩm:** phân trang, `pageSize` = 100 (biên trên) và 101 (vượt biên), sắp xếp theo giá, thời gian phản hồi.
- **Phân quyền:** gọi API quản trị khi chưa đăng nhập (401) và bằng tài khoản khách hàng (403).

Mỗi lần chạy tạo một tài khoản `autotest_<thời gian>` mới trong cơ sở dữ liệu.

### Chạy bằng Postman

1. Chạy backend (mặc định `http://localhost:5014`).
2. Import file `api/NutriMilk.postman_collection.json` vào Postman.
3. Chuột phải vào collection → **Run collection**.

### Chạy bằng Newman (dòng lệnh)

```bash
npx newman run test/api/NutriMilk.postman_collection.json --env-var baseUrl=http://localhost:5014
```
