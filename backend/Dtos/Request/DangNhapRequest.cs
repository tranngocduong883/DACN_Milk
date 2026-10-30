using System.ComponentModel.DataAnnotations;

namespace DACN_H_P.Dtos.Request
{
    public class DangNhapRequest
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string TenDangNhap { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string MatKhau { get; set; } = null!;

    }
}
