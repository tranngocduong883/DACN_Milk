namespace DACN_H_P.Dtos.Response
{
    public class TaiKhoanResponse
    {
        public string MaTaiKhoan { get; set; } = null!;
        public string TenDangNhap { get; set; } = null!;
        public string? Email { get; set; }
        public string? HoTen { get; set; }
        public string? SoDienThoai { get; set; }
        public string? VaiTro { get; set; }
        public string? DiaChi { get; set; }
    }
}
