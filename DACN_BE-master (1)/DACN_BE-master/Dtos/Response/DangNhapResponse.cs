namespace DACN_H_P.Dtos.Response
{
    public class DangNhapResponse
    {
        public string MaTaiKhoan { get; set; } = null!;

        public string TenDangNhap { get; set; } = null!;
        public string? Email { get; set; }
        public string? VaiTro { get; set; }
        public string? Token { get; set; }
    }
}
