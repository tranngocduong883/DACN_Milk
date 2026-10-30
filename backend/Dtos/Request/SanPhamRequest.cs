using System.ComponentModel.DataAnnotations;

namespace DACN_H_P.Dtos.Request
{
    public class SanPhamRequest
    {
        [Required(ErrorMessage = "Mã sản phẩm không được để trống")]
        public string MaSp { get; set; } = null!;

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(200, ErrorMessage = "Tên sản phẩm tối đa 200 ký tự")]
        public string TenSp { get; set; } = null!;

        public string? MaDanhMuc { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Gia { get; set; }

        public string? MoTa { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn không được âm")]
        public int? SoLuongTon { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Thể tích không được âm")]
        public decimal? TheTich { get; set; }

        public string? DonVi { get; set; }

        public List<IFormFile>? AnhSps { get; set; }
    }
}
