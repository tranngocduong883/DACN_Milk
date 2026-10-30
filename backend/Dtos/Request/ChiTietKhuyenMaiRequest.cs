using System.ComponentModel.DataAnnotations;

namespace DACN_H_P.Dtos.Request
{
    public class ChiTietKhuyenMaiRequest
    {
        [Required(ErrorMessage = "Mã khuyến mãi không được để trống")]
        public string MaKhuyenMai { get; set; } = null!;

        [Required(ErrorMessage = "Mã sản phẩm không được để trống")]
        public string MaSp { get; set; } = null!;

        [Range(1, 100, ErrorMessage = "Phần trăm giảm phải từ 1 đến 100")]
        public int PhanTramGiam { get; set; }
    }
}
