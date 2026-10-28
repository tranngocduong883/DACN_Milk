using DACN_H_P.Dtos.Response;
using DACN_H_P.Model;

namespace DACN_H_P.Mapper
{
    public class DonHangBuilder
    {
        public static DonHangResponse ToResponse(DonHang dh)
        {
            DateTime ngaydat = dh.NgayDatHang ?? DateTime.Now;
            DateTime ngaygiao;

            string maPTVC = dh.MaPtvc?.Trim().ToUpper() ?? "";
            decimal phishipCH = dh.MaPtvcNavigation?.PhiVanChuyen ?? 30000;
            decimal phiShipFinal;
            if (maPTVC == "PTVC01")
            {
                ngaygiao = ngaydat;
                phiShipFinal = 0;
            }
            else
            {
                ngaygiao = ngaydat.AddDays(1);
                if(dh.TongTienHang > 300000)
                {
                    phiShipFinal = 0;
                }
                else
                {
                    phiShipFinal = phishipCH;
                }
            }

            decimal giamgia = dh.MaVoucherNavigation?.GiamGia ?? 0;
            decimal thuctra = dh.TongTienHang + phiShipFinal - giamgia;
            return new DonHangResponse
            {
                MaDonHang = dh.MaDonHang,
                MaTaiKhoan = dh.MaTaiKhoan,
                TenNguoiDat = dh.MaTaiKhoanNavigation?.HoTen ?? dh.MaTaiKhoanNavigation?.TenDangNhap,
                NgayDatHang = ngaydat,
                TrangThaiDonHang = dh.TrangThaiDonHang ?? string.Empty,
                TongTienHang = dh.TongTienHang,
                DiaChiGiaoHang = dh.DiaChiGh,
                PhuongThucThanhToan = dh.MaPtttNavigation?.TenPttt ?? string.Empty,
                PhuongThucVanChuyen = dh.MaPtvcNavigation?.TenPtvc ?? string.Empty,
                PhiShip = phiShipFinal,
                GiamGiaVoucher = giamgia,
                NgayGiaoHang = ngaygiao,
                ThanhToan = thuctra,
                ChiTietDonHang = dh.ChiTietDhs.Select(ct => ChiTietDonHangBuilder.ToResponse(ct)).ToList()
            };
        }
    }
}
