# Báo cáo kiểm thử – NutriMilk

## Phạm vi

- Giai đoạn: kiểm thử tích hợp, kiểm thử hệ thống
- Kiểu kiểm thử: chức năng, giao diện người dùng
- Cách thực hiện: kiểm thử thủ công trên Google Chrome và Microsoft Edge; kiểm thử API bằng Postman

## Kết quả

- Tổng số test case: **42**
- Pass: **42**, Fail: **0**
- Tỉ lệ pass: **100%**

| STT | Chức năng | Số test case | Pass | Fail | Priority |
|---|---|---|---|---|---|
| 1 | Xem sản phẩm theo danh mục | 3 | 3 | 0 | High |
| 2 | Xem chi tiết sản phẩm | 2 | 2 | 0 | High |
| 3 | Xem lịch sử mua hàng | 4 | 4 | 0 | Medium |
| 4 | Quản lý giỏ hàng | 6 | 6 | 0 | High |
| 5 | Thanh toán | 3 | 3 | 0 | High |
| 6 | Bảo trì danh mục | 6 | 6 | 0 | High |
| 7 | Quản lý đơn hàng | 5 | 5 | 0 | High |
| 8 | Quản lý đánh giá sản phẩm | 3 | 3 | 0 | Medium |
| 9 | Bảo trì sản phẩm | 5 | 5 | 0 | High |
| 10 | Bảo trì Voucher | 5 | 5 | 0 | High |

## Đánh giá

**Ưu điểm**

- Các chức năng cốt lõi hoạt động ổn định.
- Giao diện dễ sử dụng, luồng mua hàng rõ ràng.
- Không phát hiện lỗi nghiêm trọng trong quá trình kiểm thử.

**Nhược điểm**

- Một số chức năng chưa hoàn thiện ở giao diện người dùng.
- Chưa đầy đủ các thông báo khi thêm, sửa, xoá ở một số chức năng.
