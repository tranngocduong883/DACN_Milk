using System.ComponentModel.DataAnnotations;

namespace DACN_H_P.Dtos.Request
{
    // Tham số tìm kiếm, lọc, sắp xếp và phân trang sản phẩm (đọc từ query string)
    public class SanPhamQuery
    {
        [Range(1, int.MaxValue, ErrorMessage = "Số trang phải lớn hơn 0")]
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Số sản phẩm mỗi trang phải từ 1 đến 100")]
        public int PageSize { get; set; } = 12;

        public string? MaDM { get; set; }

        public string? TuKhoa { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá không được âm")]
        public decimal? GiaTu { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá không được âm")]
        public decimal? GiaDen { get; set; }

        // gia_asc | gia_desc | ten_asc | ten_desc, mặc định sắp theo mã sản phẩm
        [RegularExpression("^(gia_asc|gia_desc|ten_asc|ten_desc)$", ErrorMessage = "Kiểu sắp xếp không hợp lệ")]
        public string? SapXep { get; set; }
    }
}
