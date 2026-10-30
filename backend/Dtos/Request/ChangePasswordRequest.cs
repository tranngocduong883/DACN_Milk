using System.ComponentModel.DataAnnotations;

namespace DACN_H_P.Dtos.Request
{
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "Mật khẩu hiện tại không được để trống")]
        public string MatKhauCu { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu mới không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu mới phải có ít nhất 6 ký tự")]
        public string MatKhauMoi { get; set; } = null!;
    }
}
