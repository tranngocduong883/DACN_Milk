namespace DACN_H_P.Dtos.Request
{
    public class DangKyRequest
    {
        public string TenDangNhap { get; set; } = null!;

        public string MatKhau { get; set; } = null!;

        public string? Email { get; set; }

        public string? HoTen { get; set; }

        public string? SoDienThoai { get; set; }
        public string? DiaChi { get; set; }
    }
}
