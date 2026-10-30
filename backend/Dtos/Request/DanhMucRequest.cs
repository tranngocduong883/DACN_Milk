using System.ComponentModel.DataAnnotations;

namespace DACN_H_P.Dtos.Request
{
    public class DanhMucRequest
    {
        [Required(ErrorMessage = "Mã danh mục không được để trống")]
        public string MaDanhMuc { get; set; } = null!;

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        public string TenDanhMuc { get; set; } = null!;

        public string? MoTa { get; set; }
    }
}
