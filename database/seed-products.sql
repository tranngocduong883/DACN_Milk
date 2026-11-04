-- Dữ liệu mẫu sản phẩm cho DACN_H (chạy sau init-db.sql).
-- Ảnh dùng đường dẫn tương đối /images/... do frontend (websiteforMilk/public/images) phục vụ.
USE DACN_H
GO
IF NOT EXISTS (SELECT 1 FROM SAN_PHAM)
BEGIN
    DECLARE @moTa NVARCHAR(MAX) = N'Sữa hoàn toàn từ sữa bò tươi (96%), đường (3,8%), hỗn hợp chất nhũ hoá và chất ổn định (E 471, E 407, E412)';

    INSERT INTO SAN_PHAM (maSP, tenSP, maDanhMuc, gia, moTa, soLuongTon, theTich, donVi)
    VALUES
    ('SP01', N'Lốc sữa tươi tiệt trùng có đường 180ml', 'DM01', 35000, @moTa, 100, 180, N'Lốc'),
    ('SP02', N'Nước cam tự nhiên true JUICE 350 ml', 'DM06', 25000, N'Nước cam ép tự nhiên, không chất bảo quản', 100, 350, N'Chai'),
    ('SP03', N'Thùng 20 Que Kem Que TOPKID Sô Cô La Nguyên Chất Phomat Tự Nhiên 46g', 'DM04', 172000, N'Kem que sô cô la nguyên chất kết hợp phomat tự nhiên', 50, 46, N'Thùng'),
    ('SP04', N'Nước dâu tự nhiên true JUICE 350 ml', 'DM06', 19000, N'Nước dâu ép tự nhiên, không chất bảo quản', 100, 350, N'Chai'),
    ('SP05', N'Thùng 48 hộp sữa tươi hương dâu 180ml', 'DM01', 428000, @moTa, 40, 180, N'Thùng'),
    ('SP06', N'Thùng 48 hộp sữa tươi hương cam 180ml', 'DM01', 428000, @moTa, 40, 180, N'Thùng'),
    ('SP07', N'Lốc Sữa Chua Ít Đường TH true YOGURT 100 g', 'DM02', 25000, N'Sữa chua ăn ít đường, lên men tự nhiên', 80, 100, N'Lốc'),
    ('SP08', N'Lốc Sữa Chua Kem Ý Đường YOGURT 100 g', 'DM02', 25000, N'Sữa chua kem phong cách Ý', 80, 100, N'Lốc'),
    ('SP09', N'Sữa tươi hương dâu Nutri 180ml', 'DM01', 9000, @moTa, 200, 180, N'Chai'),
    ('SP10', N'Sữa tươi tiệt trùng có đường 1L', 'DM01', 35000, @moTa, 120, 1000, N'Hộp');

    INSERT INTO ANH_SP (maAnh, maSP, duongDan)
    VALUES
    ('IMG01_1', 'SP01', '/images/p1.png'), ('IMG01_2', 'SP01', '/images/p6.png'), ('IMG01_3', 'SP01', '/images/p5.png'),
    ('IMG02_1', 'SP02', '/images/p2.png'), ('IMG02_2', 'SP02', '/images/p4.png'),
    ('IMG03_1', 'SP03', '/images/p3.png'),
    ('IMG04_1', 'SP04', '/images/p4.png'), ('IMG04_2', 'SP04', '/images/p2.png'),
    ('IMG05_1', 'SP05', '/images/p5.png'), ('IMG05_2', 'SP05', '/images/p6.png'),
    ('IMG06_1', 'SP06', '/images/p5.png'),
    ('IMG07_1', 'SP07', '/images/p1.png'),
    ('IMG08_1', 'SP08', '/images/p1.png'),
    ('IMG09_1', 'SP09', '/images/p6.png'), ('IMG09_2', 'SP09', '/images/p5.png'),
    ('IMG10_1', 'SP10', '/images/p1.png');

    -- Gắn vài sản phẩm vào chương trình khuyến mãi đang diễn ra
    IF EXISTS (SELECT 1 FROM KHUYEN_MAI WHERE maKhuyenMai = 'KM_MUA_HE')
        INSERT INTO CHI_TIET_KM (maKhuyenMai, maSP, phanTramGiam)
        VALUES ('KM_MUA_HE', 'SP02', 10), ('KM_MUA_HE', 'SP07', 20);
END
GO
