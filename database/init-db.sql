USE master
GO
CREATE DATABASE DACN_H
GO
USE DACN_H
GO
-- =======================================================
-- NHÓM 1: CÁC BẢNG DANH MỤC & CẤU HÌNH (Độc lập)
-- =======================================================

CREATE TABLE TAI_KHOAN (
    maTaiKhoan VARCHAR(50) PRIMARY KEY,
    tenDangNhap VARCHAR(100) NOT NULL UNIQUE,
    matKhau VARCHAR(255) NOT NULL,
    email VARCHAR(100) UNIQUE,
    hoTen NVARCHAR(100),
    soDienThoai VARCHAR(20),
    vaiTro NVARCHAR(50), -- 'admin', 'user'
    diaChi NVARCHAR(MAX)
);

CREATE TABLE DANH_MUC (
    maDanhMuc VARCHAR(50) PRIMARY KEY,
    tenDanhMuc NVARCHAR(100) NOT NULL,
    moTa NVARCHAR(MAX)
);

CREATE TABLE PHUONG_THUC_VC (
    maPTVC VARCHAR(50) PRIMARY KEY,
    tenPTVC NVARCHAR(100) NOT NULL,
    phiVanChuyen DECIMAL(18, 2) NOT NULL DEFAULT 0 -- Đã chuẩn hóa về Decimal(18,2)
);

CREATE TABLE PHUONG_THUC_TT (
    maPTTT VARCHAR(50) PRIMARY KEY,
    tenPTTT NVARCHAR(100) NOT NULL
);

CREATE TABLE VOUCHER (
    maVoucher VARCHAR(50) PRIMARY KEY,
    tenVoucher NVARCHAR(100),
    giamGia DECIMAL(18, 2) NOT NULL DEFAULT 0, -- Đã đổi từ phanTramGiam sang giamGia (tiền mặt)
    ngayBatDau DATETIME NOT NULL,
    ngayKetThuc DATETIME NOT NULL,
    dieuKienApDung DECIMAL(18, 0), -- Đã đổi từ String sang Decimal để so sánh tiền
    moTa NVARCHAR(MAX) -- Đã thêm mới
);

CREATE TABLE KHUYEN_MAI (
    maKhuyenMai VARCHAR(50) PRIMARY KEY,
    tenKhuyenMai NVARCHAR(100) NOT NULL,
    ngayBatDau DATETIME NOT NULL,
    ngayKetThuc DATETIME NOT NULL,
    moTa NVARCHAR(MAX)
);

CREATE TABLE CUA_HANG (
    maCuaHang VARCHAR(50) PRIMARY KEY,
    tenCuaHang NVARCHAR(100) NOT NULL,
    diaChi NVARCHAR(MAX),
    soDienThoai VARCHAR(20),
    email VARCHAR(100),
    moTa NVARCHAR(MAX)
);

-- =======================================================
-- NHÓM 2: SẢN PHẨM & LIÊN KẾT (Phụ thuộc Nhóm 1)
-- =======================================================

CREATE TABLE SAN_PHAM (
    maSP VARCHAR(50) PRIMARY KEY,
    tenSP NVARCHAR(100) NOT NULL, -- Đã thêm cột này vào lúc tạo bảng
    maDanhMuc VARCHAR(50),
    gia DECIMAL(18, 2) NOT NULL,
    moTa NVARCHAR(MAX),
    soLuongTon INT DEFAULT 0,
    theTich DECIMAL(10, 2),
    donVi NVARCHAR(50),
    FOREIGN KEY (maDanhMuc) REFERENCES DANH_MUC(maDanhMuc)
);

CREATE TABLE ANH_SP (
    maAnh VARCHAR(50) PRIMARY KEY,
    maSP VARCHAR(50) NOT NULL,
    duongDan VARCHAR(255) NOT NULL,
    FOREIGN KEY (maSP) REFERENCES SAN_PHAM(maSP) ON DELETE CASCADE
);

CREATE TABLE CHI_TIET_KM (
    maKhuyenMai VARCHAR(50),
    maSP VARCHAR(50),
    phanTramGiam INT NOT NULL CHECK (phanTramGiam > 0 AND phanTramGiam <= 100),
    PRIMARY KEY (maKhuyenMai, maSP),
    FOREIGN KEY (maKhuyenMai) REFERENCES KHUYEN_MAI(maKhuyenMai) ON DELETE CASCADE,
    FOREIGN KEY (maSP) REFERENCES SAN_PHAM(maSP) ON DELETE CASCADE
);

-- =======================================================
-- NHÓM 3: NGHIỆP VỤ MUA HÀNG (Phụ thuộc Nhóm 1 & 2)
-- =======================================================

CREATE TABLE GIO_HANG (
    maGioHang VARCHAR(50) PRIMARY KEY,
    maTaiKhoan VARCHAR(50) UNIQUE,
    FOREIGN KEY (maTaiKhoan) REFERENCES TAI_KHOAN(maTaiKhoan) ON DELETE CASCADE
);

CREATE TABLE CHI_TIET_GH (
    maGioHang VARCHAR(50),
    maSP VARCHAR(50),
    soLuong INT NOT NULL DEFAULT 1,
    PRIMARY KEY (maGioHang, maSP),
    FOREIGN KEY (maGioHang) REFERENCES GIO_HANG(maGioHang) ON DELETE CASCADE,
    FOREIGN KEY (maSP) REFERENCES SAN_PHAM(maSP) ON DELETE CASCADE
);

CREATE TABLE DON_HANG (
    maDonHang VARCHAR(50) PRIMARY KEY,
    maTaiKhoan VARCHAR(50) NOT NULL,
    maPTTT VARCHAR(50),
    maPTVC VARCHAR(50),
    maVoucher VARCHAR(50) NULL,
    tongTienHang DECIMAL(18, 2) NOT NULL, -- Tổng tiền hàng (chưa trừ voucher/ship)
    trangThaiDonHang NVARCHAR(50), -- 'ChoXacNhan', 'DangVanChuyen'...
    ngayDatHang DATETIME DEFAULT CURRENT_TIMESTAMP,
    diaChiGH NVARCHAR(MAX) NOT NULL,
    
    FOREIGN KEY (maTaiKhoan) REFERENCES TAI_KHOAN(maTaiKhoan), -- Không Cascade để giữ lịch sử
    FOREIGN KEY (maPTTT) REFERENCES PHUONG_THUC_TT(maPTTT),
    FOREIGN KEY (maPTVC) REFERENCES PHUONG_THUC_VC(maPTVC),
    FOREIGN KEY (maVoucher) REFERENCES VOUCHER(maVoucher)
);

CREATE TABLE CHI_TIET_DH (
    maDonHang VARCHAR(50),
    maSP VARCHAR(50),
    soLuong INT NOT NULL,
    donGiaLucDat DECIMAL(18, 2) NOT NULL, -- Lưu giá tại thời điểm mua
    PRIMARY KEY (maDonHang, maSP),
    FOREIGN KEY (maDonHang) REFERENCES DON_HANG(maDonHang) ON DELETE CASCADE,
    FOREIGN KEY (maSP) REFERENCES SAN_PHAM(maSP)
);

CREATE TABLE DANH_GIA_SP (
    maDanhGia VARCHAR(50) PRIMARY KEY,
    maSP VARCHAR(50),
    maTaiKhoan VARCHAR(50),
    soSao INT CHECK (soSao BETWEEN 1 AND 5),
    binhLuan NVARCHAR(MAX),
    ngayCapNhat DATETIME DEFAULT CURRENT_TIMESTAMP,
    trangThaiDG NVARCHAR(50), -- 'ChoDuyet', 'DaDuyet', 'KhongHopLe'
    
    FOREIGN KEY (maSP) REFERENCES SAN_PHAM(maSP) ON DELETE CASCADE,
    FOREIGN KEY (maTaiKhoan) REFERENCES TAI_KHOAN(maTaiKhoan)
);
GO
DECLARE @PasswordHash NVARCHAR(255) = 'AQAAAAIAAYagAAAAEDclv2U+A08jBteEouFPYJg7Ys7TgjrZXdH1pdMeGLUWwKP+zTrKnBZZ3uNNVCFg+w==';

INSERT INTO TAI_KHOAN (maTaiKhoan, tenDangNhap, matKhau, email, hoTen, soDienThoai, vaiTro, diaChi)
VALUES 
(
    'ADMIN_KEY_01', 
    'admin1', 
    @PasswordHash, 
    'admin1@gmail.com', 
    N'Quản Trị Viên 1', 
    '0901000001', 
    'admin', 
    N'Hà Nội'
),
(
    'ADMIN_KEY_02', 
    'admin2', 
    @PasswordHash, 
    'admin2@gmail.com', 
    N'Quản Trị Viên 2', 
    '0901000002', 
    'admin', 
    N'Đà Nẵng'
),
(
    'ADMIN_KEY_03', 
    'admin3', 
    @PasswordHash, 
    'admin3@gmail.com', 
    N'Quản Trị Viên 3', 
    '0901000003', 
    'admin', 
    N'TP. Hồ Chí Minh'
),
(
    'ADMIN_KEY_04', 
    'admin4', 
    @PasswordHash, 
    'admin4@gmail.com', 
    N'Quản Trị Viên 4', 
    '0901000004', 
    'admin', 
    N'Hải Phòng'
),
(
    'ADMIN_KEY_05', 
    'admin5', 
    @PasswordHash, 
    'admin5@gmail.com', 
    N'Quản Trị Viên 5', 
    '0901000005', 
    'admin', 
    N'Cần Thơ'
);
GO
INSERT INTO DANH_MUC (maDanhMuc, tenDanhMuc, moTa)
VALUES 
('DM01', N'Sữa Tươi', N'Sữa tươi tiệt trùng, thanh trùng nguyên chất'),
('DM02', N'Sữa Chua', N'Sữa chua ăn, sữa chua uống các loại'),
('DM03', N'Sữa Hạt', N'Sữa hạt macca, óc chó, hạnh nhân'),
('DM04', N'Kem', N'Kem que, kem ốc quế, kem hộp'),
('DM05', N'Bơ & Phomat', N'Bơ lạt, phomat tự nhiên'),
('DM06', N'Nước Giải Khát', N'Nước trái cây, nước uống thảo dược');
GO
INSERT INTO PHUONG_THUC_VC (maPTVC, tenPTVC, phiVanChuyen)
VALUES 
('PTVC01', N'Nhận tại Cửa Hàng', 0),
('PTVC02', N'Giao hàng Tại Nhà', 30000);
GO
INSERT INTO PHUONG_THUC_TT (maPTTT, tenPTTT)
VALUES 
('PTTT01', N'Thanh toán khi nhận hàng (COD)'),
('PTTT02', N'Thanh toán qua VNPAY');
GO
INSERT INTO VOUCHER (maVoucher, tenVoucher, giamGia, dieuKienApDung, ngayBatDau, ngayKetThuc, moTa)
VALUES 
('WELCOME', N'Chào bạn mới', 50000, 0, GETDATE(), DATEADD(day, 30, GETDATE()), N'Giảm 50k cho đơn 0đ'),
('GIAM10K', N'Giảm nhẹ 10k', 10000, 100000, GETDATE(), DATEADD(day, 30, GETDATE()), N'Giảm 10k cho đơn từ 100k'),
('GIAM20K', N'Siêu sale tháng 12', 20000, 200000, GETDATE(), DATEADD(day, 7, GETDATE()), N'Giảm 20k cho đơn từ 200k'),
('FREESHIP', N'Hỗ trợ phí ship', 30000, 300000, GETDATE(), DATEADD(day, 60, GETDATE()), N'Trừ 30k phí ship cho đơn từ 300k'),
('VIP500', N'Voucher Khách VIP', 100000, 1000000, GETDATE(), DATEADD(day, 365, GETDATE()), N'Giảm 100k cho đơn tiền triệu');
GO
INSERT INTO KHUYEN_MAI (maKhuyenMai, tenKhuyenMai, ngayBatDau, ngayKetThuc, moTa)
VALUES 
('KM_MUA_HE', N'Lễ hội Sữa Mùa Hè', DATEADD(day, -5, GETDATE()), DATEADD(day, 5, GETDATE()), N'Giảm giá cực sốc chào hè'),
('KM_TET_2025', N'Sắm Tết TH True Mart', DATEADD(day, 10, GETDATE()), DATEADD(day, 30, GETDATE()), N'Khuyến mãi đặc biệt dịp Tết Nguyên Đán');
GO
-- Khu vực HÀ NỘI (4 cái)
INSERT INTO CUA_HANG (maCuaHang, tenCuaHang, diaChi, soDienThoai, email, moTa)
VALUES 
('CH_HN_01', N'TH True Mart Cầu Giấy', N'Số 123 Cầu Giấy, P. Quan Hoa, Q. Cầu Giấy, Hà Nội', '0243111222', 'ch_caugiay@th.com', N'Cửa hàng flagship'),
('CH_HN_02', N'TH True Mart Hoàn Kiếm', N'Số 88 Hàng Bài, Q. Hoàn Kiếm, Hà Nội', '0243333444', 'ch_hoankiem@th.com', N'Gần bờ hồ'),
('CH_HN_03', N'TH True Mart Hà Đông', N'Số 10 Nguyễn Trãi, Q. Hà Đông, Hà Nội', '0245555666', 'ch_hadong@th.com', NULL),
('CH_HN_04', N'TH True Mart Tây Hồ', N'Số 55 Lạc Long Quân, Q. Tây Hồ, Hà Nội', '0247777888', 'ch_tayho@th.com', N'View hồ tây');
-- Khu vực ĐÀ NẴNG (4 cái)
INSERT INTO CUA_HANG (maCuaHang, tenCuaHang, diaChi, soDienThoai, email, moTa)
VALUES 
('CH_DN_01', N'TH True Mart Hải Châu', N'Số 22 Lê Duẩn, Q. Hải Châu, Đà Nẵng', '0236111222', 'ch_haichau@th.com', NULL),
('CH_DN_02', N'TH True Mart Sơn Trà', N'Số 45 Ngô Quyền, Q. Sơn Trà, Đà Nẵng', '0236333444', 'ch_sontra@th.com', N'Gần cầu rồng'),
('CH_DN_03', N'TH True Mart Thanh Khê', N'Số 100 Điện Biên Phủ, Q. Thanh Khê, Đà Nẵng', '0236555666', 'ch_thanhkhe@th.com', NULL),
('CH_DN_04', N'TH True Mart Ngũ Hành Sơn', N'Số 12 Lê Văn Hiến, Q. Ngũ Hành Sơn, Đà Nẵng', '0236777888', 'ch_nhs@th.com', NULL);
-- Khu vực TP. HỒ CHÍ MINH (4 cái)
INSERT INTO CUA_HANG (maCuaHang, tenCuaHang, diaChi, soDienThoai, email, moTa)
VALUES 
('CH_HCM_01', N'TH True Mart Quận 1', N'Số 55 Nguyễn Huệ, P. Bến Nghé, Quận 1, TP.HCM', '0281111222', 'ch_q1@th.com', N'Phố đi bộ'),
('CH_HCM_02', N'TH True Mart Bình Thạnh', N'Số 200 Xô Viết Nghệ Tĩnh, Q. Bình Thạnh, TP.HCM', '0283333444', 'ch_binhthanh@th.com', NULL),
('CH_HCM_03', N'TH True Mart Quận 7', N'Số 10 Nguyễn Văn Linh, Q.7, TP.HCM', '0285555666', 'ch_q7@th.com', N'Khu Phú Mỹ Hưng'),
('CH_HCM_04', N'TH True Mart Thủ Đức', N'Số 99 Võ Văn Ngân, TP. Thủ Đức, TP.HCM', '0287777888', 'ch_thuduc@th.com', NULL);