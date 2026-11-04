# Test Case – NutriMilk

Test case kiểm thử thủ công (manual) cho 10 chức năng chính, trích từ chương 4 của [báo cáo đồ án](../docs/BaoCao_DoAn_NutriMilk.pdf).

- Tổng số test case: **42**
- Kiểu kiểm thử: chức năng, giao diện
- Môi trường: Google Chrome, Microsoft Edge
- Bản Excel: [test-cases.csv](test-cases.csv)

## Mục lục

1. [Xem sản phẩm theo danh mục](#1-xem-sản-phẩm-theo-danh-mục) – 3 case, High
2. [Xem chi tiết sản phẩm](#2-xem-chi-tiết-sản-phẩm) – 2 case, High
3. [Xem lịch sử mua hàng](#3-xem-lịch-sử-mua-hàng) – 4 case, Medium
4. [Quản lý giỏ hàng](#4-quản-lý-giỏ-hàng) – 6 case, High
5. [Thanh toán](#5-thanh-toán) – 3 case, High
6. [Bảo trì danh mục](#6-bảo-trì-danh-mục) – 6 case, High
7. [Quản lý đơn hàng](#7-quản-lý-đơn-hàng) – 5 case, High
8. [Quản lý đánh giá sản phẩm](#8-quản-lý-đánh-giá-sản-phẩm) – 3 case, Medium
9. [Bảo trì sản phẩm](#9-bảo-trì-sản-phẩm) – 5 case, High
10. [Bảo trì Voucher](#10-bảo-trì-voucher) – 5 case, High

## 1. Xem sản phẩm theo danh mục

Priority: **High**

| ID | Test case | Điều kiện | Các bước thực hiện | Kết quả mong đợi | Tham số | Kết quả |
|---|---|---|---|---|---|---|
| XSPTDM1 | Hiển thị danh mục sản phẩm | Không | 1. Truy cập trang chủ.<br>2. Nhấn “Danh mục” | Hiển thị đầy đủ tên các danh mục từ bảng DANH_MUC | Không | Pass |
| XSPTDM2 | Hiển thị sản phẩm theo danh mục | Danh mục có sản phẩm | 1. Chọn danh mục | Hiển thị danh sách sản phẩm | Không | Pass |
| XSPTDM3 | Hiển thị danh mục trống | Danh mục không có sản phẩm | 1. Chọn danh mục | Xuất hiện thông báo: “Không có sản phẩm trong danh mục này” | Không | Pass |

## 2. Xem chi tiết sản phẩm

Priority: **High**

| ID | Test case | Điều kiện | Các bước thực hiện | Kết quả mong đợi | Tham số | Kết quả |
|---|---|---|---|---|---|---|
| XCTSP1 | Hiển thị thông tin chi tiết sản phẩm | Sản phẩm tồn tại trong bảng SAN_PHAM | 1. Nhấn vào 1 sản phẩm bất kỳ | Hiển thị đầy đủ tên các thông tin về sản phẩm, ảnh sản phẩm, các khuyến mại của sản phẩm | Mã sản phẩm | Pass |
| XCTSP2 | Không tìm thấy sản phẩm | Sản phẩm không có trong bảng SAN_PHAM | 1. Truy cập chi tiết sản phẩm với mã sản phẩm không tồn tại. | Hiển thị thông báo “Không tìm thấy sản phẩm” | Mã sản phẩm không hợp lệ | Pass |

## 3. Xem lịch sử mua hàng

Priority: **Medium**

| ID | Test case | Điều kiện | Các bước thực hiện | Kết quả mong đợi | Tham số | Kết quả |
|---|---|---|---|---|---|---|
| XLSMH1 | Hiển thị danh sách lịch sử mua hàng | Khách hàng đăng nhập và đã mua hàng | 1. Truy cập mục “Lịch sử mua hàng” | Hiển thị đầy đủ các thông tin về đơn hàng | Không | Pass |
| XLSMH2 | Không có đơn hàng nào | Khách hàng đăng nhập và chưa từng mua hàng | 1. Truy cập mục “Lịch sử mua hàng” | Hiển thị thông báo “Không có đơn hàng nào” | Không | Pass |
| XLSMH3 | Xem chi tiết đơn hàng | Khách hàng đăng nhập và đã mua hàng | 1. Mở “Lịch sử mua hàng”.<br>2. Nhấn vào mã đơn bất kỳ | Thông tin sản phẩm đã mua, hình ảnh, số lượng, giá bán | Không | Pass |
| XLSMH4 | Mua lại đơn hàng | Trạng thái đơn hàng là “Hoàn tátt” | 1. Mở “Lịch sử mua hàng”.<br>2. Nhấn vào mã đơn bất kỳ<br>3.Nhấn nút “Mua lại” | Toàn bộ sản phẩm trong đơn được thêm vào giỏ hàng | Không | Pass |

## 4. Quản lý giỏ hàng

Priority: **High**

| ID | Test case | Điều kiện | Các bước thực hiện | Kết quả mong đợi | Tham số | Kết quả |
|---|---|---|---|---|---|---|
| QLGH1 | Hiển thị giỏ hàng với sản phẩm | Giỏ hàng có ít nhất 1 sản phẩm | 1. Truy cập trang chủ.<br>2. Nhấn “Giỏ hàng” | Hiển thị đầy đủ ảnh sản phẩm, tên sản phẩm, giá, số lượng mua | Không | Pass |
| QLGH2 | Giỏ hàng trống | Giỏ hàng chưa có sản phẩm | 1. Truy cập trang chủ.<br>2. Nhấn “Giỏ hàng” | Hiển thị thông báo “Hiện chưa có sản phẩm nào trong giỏ hàng” | Không | Pass |
| QLGH3 | Thay đổi số lượng sản phẩm | Sản phẩm còn tồn tại | 1. Truy cập trang chủ.<br>2. Nhấn “Giỏ hàng”.<br>3. Thay đổi số lượng sản phẩm | Cập nhật số lượng theo sản phẩm, Tổng tiền được cập nhật. | Không | Pass |
| QLGH4 | Thêm sản phẩm vào giỏ hàng | Không | 1. Vào chi tiết sản phẩm<br>2. Nhấn “Thêm vào giỏ hàng”. | Hiển thị thông báo “Thêm vào giỏ hàng thành công”. Cập nhật giỏ hàng | Không | Pass |
| QLGH5 | Đồng ý xoá sản phẩm khỏi giỏ hàng | Giỏ hàng có sản phẩm | 1. Vào giỏ hàng<br>2. Nhấn biểu tượng “Thùng rác” tại sản phẩm.<br>3. Chọn “Đồng ý” | Sản phẩm bị xoá khỏi giỏ. Cập nhật lại giỏ hàng. | Không | Pass |
| QLGH6 | Huỷ xoá sản phẩm khỏi giỏ hàng | Giỏ hàng có sản phẩm | 1. Vào giỏ hàng<br>2. Nhấn biểu tượng “Thùng rác” tại sản phẩm.<br>3. Chọn “Huỷ” | Sản phẩm không bị xoá khỏi giỏ hàng. | Không | Pass |

## 5. Thanh toán

Priority: **High**

| ID | Test case | Điều kiện | Các bước thực hiện | Kết quả mong đợi | Tham số | Kết quả |
|---|---|---|---|---|---|---|
| TT1 | Thanh toán thành công | Khách hàng đã đăng nhập. Giỏ hàng có sản phẩm | 1. Vào “Giỏ hàng”, chọn “Thanh toán”.<br>2. Chọn phương thức vận chuyển, phương thức thanh toán.<br>3. Nhấn “Xác nhận đơn hàng” | Tạo bản ghi mới trong DON_HANG, CHI_TIET_DH. Hiển thị thông báo đặt hàng thành công. | Không | Pass |
| TT2 | Áp dụng Voucher hợp lệ | Voucher thoả mãn | 1. Vào “Giỏ hàng”, chọn “Thanh toán”.<br>2. Nhập Voucher | Hiển thị tổng tiền sau khi áp dụng Voucher. | Không | Pass |
| TT3 | Áp dụng Voucher không hợp lệ | Voucher không thoả mãn | 1. Vào “Giỏ hàng”, chọn “Thanh toán”.<br>2. Nhập Voucher | Thông báo lỗi “Voucher không hợp lệ” | Không | Pass |

## 6. Bảo trì danh mục

Priority: **High**

| ID | Test case | Điều kiện | Các bước thực hiện | Kết quả mong đợi | Tham số | Kết quả |
|---|---|---|---|---|---|---|
| BTDM1 | Xem danh sách danh mục | Quản trị viên đã đăng nhập | 1. Chọn chức năng “Quản lý danh mục” | Hiển thị đầy đủ tên các danh mục từ bảng DANH_MUC | Không | Pass |
| BTDM2 | Thêm danh mục mới thành công | Quản trị viên đã đăng nhập | 1. Chọn chức năng “Quản lý danh mục”<br>2. Nhấn “Thêm mới”.<br>3. Nhập các thông tin hợp lệ.<br>4. Nhấn “Tạo mới” | Thêm bản ghi mới vào DANH_MUC. Cập nhật lại danh sách. | Mã danh mục, tên danh mục, mô tả | Pass |
| BTDM3 | Thêm danh mục mới thất bại | Quản trị viên đã đăng nhập | 1. Chọn chức năng “Quản lý danh mục”<br>2. Nhấn “Thêm mới”.<br>3. Nhập trùng mã danh mục<br>4. Nhấn “Tạo mới” | Thông báo lỗi “Mã danh mục đã tồn tại” | Mã danh mục, tên danh mục, mô tả | Pass |
| BTDM4 | Sửa danh mục | Quản trị viên đã đăng nhập. Tồn tại ít nhất 1 danh mục | 1. Chọn chức năng “Quản lý danh mục”<br>2. Nhấn “Sửa”.<br>3. Sửa các thông tin hợp lệ.<br>4. Nhấn “Cập nhật” | Lưu bản ghi mới vào DANH_MUC. Cập nhật lại danh sách. | Mã danh mục, tên danh mục, mô tả | Pass |
| BTDM5 | Xoá danh mục thành công | Quản trị viên đã đăng nhập. Danh mục không chứa sản phẩm | 1. Chọn chức năng “Quản lý danh mục”<br>2. Nhấn “Xoá” trên 1 danh mục.<br>3. Nhấn “Đồng ý” | Danh mục bị xoá khỏi DANH_MUC. Cập nhật lại danh sách. | Không | Pass |
| BTDM6 | Xoá danh mục thất bại | Quản trị viên đã đăng nhập. Tồn tại sản phẩm thuộc danh mục | 1. Chọn chức năng “Quản lý danh mục”<br>2. Nhấn “Xoá” trên 1 danh mục<br>3. Nhấn “Đồng ý” | Thông báo lỗi “Danh mục còn sản phẩm, không thể xoá”. | Không | Pass |

## 7. Quản lý đơn hàng

Priority: **High**

| ID | Test case | Điều kiện | Các bước thực hiện | Kết quả mong đợi | Tham số | Kết quả |
|---|---|---|---|---|---|---|
| QLDH1 | Xem danh sách đơn hàng | Quản trị viên đã đăng nhập. | 1. Chọn chức năng “Quản lý đơn hàng” | Hiển thị danh sách đơn hàng gồm mã đơn, tổng tiền, ngày đặt, trạng thái từ bảng DON_HANG | Không | Pass |
| QLDH2 | Cập nhật trạng thái đơn hàng thành công | Quản trị viên đã đăng nhập. Trạng thái mới hợp lệ theo quy định | 1. Chọn chức năng “Quản lý đơn hàng”<br>2. Chọn đơn hàng<br>3. Thay đổi trạng thái hợp lệ<br>4. Nhấn “Cập nhật” | Cập nhật dữ liệu vào DON_HANG. Hiển thị lại trạng thái mới. | Không | Pass |
| QLDH3 | Cập nhật trạng thái đơn hàng thất bại | Quản trị viên đã đăng nhập. Trạng thái mới không hợp lệ theo quy định | 1. Chọn chức năng “Quản lý đơn hàng”<br>2. Chọn đơn hàng<br>3. Thay đổi trạng thái không hợp lệ.<br>4. Nhấn “Cập nhật” | Thông báo lỗi “Trạng thái không hợp lệ” | Không | Pass |
| QLDH4 | Xoá đơn hàng thành công | Quản trị viên đã đăng nhập. Trạng thái hợp lệ để xoá | 1. Chọn chức năng “Quản lý đơn hàng”<br>2. Chọn “Xoá” trên đơn hàng<br>3. Nhấn “Đồng ý” | Xoá đơn hàng khỏi DON_HANG. Hiển thị lại danh sách. | Không | Pass |
| QLDH5 | Xoá đơn hàng thất bại | Quản trị viên đã đăng nhập. Trạng thái không hợp lệ để xoá | 1. Chọn chức năng “Quản lý đơn hàng”<br>2. Chọn “Xoá” trên đơn hàng<br>3. Nhấn “Đồng ý” | Thông báo lỗi “Không thể xoá đơn hàng đang xử lý/đang giao”. | Không | Pass |

## 8. Quản lý đánh giá sản phẩm

Priority: **Medium**

| ID | Test case | Điều kiện | Các bước thực hiện | Kết quả mong đợi | Tham số | Kết quả |
|---|---|---|---|---|---|---|
| QLDGSP1 | Hiển thị danh sách đánh giá sản phẩm | Quản trị viên đã đăng nhập | 1. Chọn chức năng “Quản lý đánh giá sản phẩm” | Hiển thị danh sách đánh giá với các thông tin từ bảng DANH_GIA. | Không | Pass |
| QLDGSP2 | Duyệt đánh giá sản phẩm | Quản trị viên đã đăng nhập | 1. Chọn chức năng “Quản lý đánh giá sản phẩm”<br>2. Nhấn “Duyệt” trên 1 đánh giá<br>3. Nhấn đồng ý | Cập nhật trạng thái đánh giá trong DANH_GIA. Hiển thị lại danh sách | Không | Pass |
| QLDGSP3 | Xoá đánh giá sản phẩm | Quản trị viên đã đăng nhập | 1. Chọn chức năng “Quản lý đánh giá sản phẩm”<br>2. Chọn “Xoá” trên 1 đánh giá.<br>3. Nhấn “Đồng ý” | Đánh giá được xoá khỏi bảng DANH_GIA. Hiển thị lại danh sách. | Không | Pass |

## 9. Bảo trì sản phẩm

Priority: **High**

| ID | Test case | Điều kiện | Các bước thực hiện | Kết quả mong đợi | Tham số | Kết quả |
|---|---|---|---|---|---|---|
| BTSP1 | Hiển thị danh sách sản phẩm | Quản trị viên đã đăng nhập | 1. Chọn chức năng “Quản lý sản phẩm” | Hiển thị đầy đủ thông tin chính về sản phẩm từ bảng SAN_PHAM | Không | Pass |
| BTSP2 | Thêm mới sản phẩm thành công | Quản trị viên đã đăng nhập. Mã sản phẩm mới chưa tồn tại | 1. Chọn chức năng “Quản lý sản phẩm”<br>2. Nhấn “Thêm mới”<br>3. Nhập các thông tin hợp lệ<br>4. Nhấn “Tạo mới” | Bản ghi mới được thêm vào SAN_PHAM. Cập nhật lại danh sách | Mã sản phẩm, tên sản phẩm. giá, … | Pass |
| BTSP3 | Thêm mới sản phẩm thất bại | Quản trị viên đã đăng nhập. Mã sản phẩm mới đã tồn tại | 1. Chọn chức năng “Quản lý sản phẩm”<br>2. Nhấn “Thêm mới”<br>3. Nhập mã sản phẩm đã tồn tại<br>3. Nhấn “Tạo mới” | Thông báo lỗi “Mã sản phẩm đã tồn tại” | Mã sản phẩm, tên sản phẩm. giá, … | Pass |
| BTSP4 | Sửa sản phẩm | Quản trị viên đã đăng nhập. Sản phẩm đã tồn tại | 1. Chọn chức năng “Quản lý sản phẩm”<br>2. Nhấn “Sửa” trên 1 sản phẩm<br>3. Nhập các thông tin<br>4. Nhấn “Cập nhật” | Bản ghi được sửa vào bảng SAN_PHAM. Cập nhật lại danh sách | Mã sản phẩm, tên sản phẩm. giá, … | Pass |
| BTSP5 | Xoá sản phẩm | Quản trị viên đã đăng nhập. Tồn tại ít nhất 1 sản phẩm | 1. Chọn chức năng “Quản lý sản phẩm”<br>2. Chọn “Xoá” trên 1 dòng sản phẩm<br>3. Nhấn “Đồng ý” | Xoá sản phẩm khỏi bảng SAN_PHAM. Cập nhật lại danh sách | Không | Pass |

## 10. Bảo trì Voucher

Priority: **High**

| ID | Test case | Điều kiện | Các bước thực hiện | Kết quả mong đợi | Tham số | Kết quả |
|---|---|---|---|---|---|---|
| BTVC1 | Hiển thị danh sách Voucher | Quản trị viên đã đăng nhập | 1. Chọn chức năng “Quản lý Voucher” | Hiển thị đầy đủ thông tin của Voucher từ bảng VOUCHER | Không | Pass |
| BTVC2 | Thêm mới Voucher thành công | Quản trị viên đã đăng nhập | 1. Chọn chức năng “Quản lý Voucher”<br>2. Nhấn “Thêm mới”<br>3. Nhập thông tin hợp lệ<br>4. Nhấn “Tạo mới” | Tạo bản ghi mới vào bảng VOUCHER. Cập nhật lại danh sách | Mã Voucher, phần trăm giảm, ngày bắt đầu, ngày kết thúc, điều kiện áp dụng. | Pass |
| BTVC3 | Thêm mới Voucher thất bại | Quản trị viên đã đăng nhập. Thông tin nhập vào không hợp lệ | 1. Chọn chức năng “Quản lý Voucher”<br>2. Nhấn “Thêm mới”<br>3. Nhập thông tin không hợp lệ (trùng mã voucher, ngày kết thúc < ngày bắt đầu)<br>4. Nhấn “Tạo mới” | Thông báo lỗi “Mã voucher đã tồn tại”/“Ngày bắt đầu phải trước ngày kết thúc” | Mã Voucher, phần trăm giảm, ngày bắt đầu, ngày kết thúc, điều kiện áp dụng. | Pass |
| BTVC4 | Sửa Voucher | Quản trị viên đã đăng nhập | 1. Chọn chức năng “Quản lý Voucher”<br>2. Nhấn “Sửa” trên 1 dòng voucher<br>3. Nhập thông tin hợp lệ<br>4. Nhấn “Cập nhật” | Cập nhật bản ghi mới vào bảng VOUCHER. Cập nhật lại danh sách | Mã Voucher, phần trăm giảm, ngày bắt đầu, ngày kết thúc, điều kiện áp dụng. | Pass |
| BTVC5 | Xoá voucher | Quản trị viên đã đăng nhập. Tồn tại ít nhất 1 voucher | 1. Chọn chức năng “Quản lý Voucher”<br>2. Chọn “Xoá” trên 1 dòng voucher<br>3. Nhấn “Đồng ý” | Xoá voucher khỏi bảng VOUCHER. Cập nhật lại danh sách | Không | Pass |
