using System.ComponentModel.DataAnnotations;

namespace DACN_H_P.Dtos.Request
{
    public class DatHangRequest
    {
        [Required(ErrorMessage = "Mã tài khoản không được để trống")]
        public string MaTaiKhoan { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        public string MaPTTT { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng chọn phương thức vận chuyển")]
        public string MaPTVC { get; set; } = null!;

        public string? MaVoucher { get; set; }

        [Required(ErrorMessage = "Địa chỉ giao hàng không được để trống")]
        public string DiaChiGiaoHang { get; set; } = null!;
    }
}
