using DACN_H_P.Dtos.Response;
using DACN_H_P.Model;

namespace DACN_H_P.Mapper
{
    public class DanhGiaBuilder
    {
        public static DanhGiaResponse ToResponse(DanhGiaSp dg)
        {
            return new DanhGiaResponse
            {
                MaDanhGia = dg.MaDanhGia,
                MaSp = dg.MaSp ?? "",
                TenSp = dg.MaSpNavigation?.TenSp ?? "Sản phẩm đã xóa",
                MaTaiKhoan = dg.MaTaiKhoan ?? "",
                TenNguoiDung = dg.MaTaiKhoanNavigation?.HoTen ?? dg.MaTaiKhoanNavigation?.TenDangNhap ?? "Người dùng ẩn danh",
                SoSao = dg.SoSao ?? 5,
                BinhLuan = dg.BinhLuan ?? "",
                NgayCapNhat = dg.NgayCapNhat ?? DateTime.Now,
                TrangThai = dg.TrangThaiDg ?? "ChoDuyet"
            };
        }
    }
}
