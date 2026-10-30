using System.ComponentModel.DataAnnotations;

namespace DACN_H_P.Dtos.Request
{
    public class ChiTietGioHangRequest
    {
        [Required(ErrorMessage = "Mã sản phẩm không được để trống")]
        public string MaSP { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuong { get; set; }
    }
}
