using System.ComponentModel.DataAnnotations;

namespace DACN_H_P.Dtos.Request
{
    public class TaoDanhGiaRequest
    {
        [Required]
        public string MaSp { get; set; } = null!;
        [Required]
        public string MaTaiKhoan { get; set; } = null!;
        [Range(1, 5, ErrorMessage = "Số sao phải từ 1 đến 5")]
        public int SoSao { get; set; }
        public string? BinhLuan { get; set; }
    }

    public class SuaDanhGiaRequest
    {
        [Required]
        public string MaDanhGia { get; set; } = null!;
        [Required]
        public string MaTaiKhoan { get; set; } = null!;
        [Range(1, 5)]
        public int SoSao { get; set; }
        public string? BinhLuan { get; set; }
    }

    public class DuyetDanhGiaRequest
    {
        [Required]
        public string TrangThai { get; set; } = null!;
    }
}
