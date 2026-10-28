using DACN_H_P.Dtos.Response;
using DACN_H_P.Model;

namespace DACN_H_P.Mapper
{
    public class TaiKhoanBuilder
    {
        public static TaiKhoanResponse ToResponse(TaiKhoan tk)
        {
            return new TaiKhoanResponse
            {
                MaTaiKhoan = tk.MaTaiKhoan,
                TenDangNhap = tk.TenDangNhap,
                Email = tk.Email,
                HoTen = tk.HoTen,
                SoDienThoai = tk.SoDienThoai,
                VaiTro = tk.VaiTro,
                DiaChi = tk.DiaChi
            };
        }
    }
}
