namespace DACN_H_P.Dtos.Request
{
    public class SanPhamRequest
    {
        public string MaSp { get; set; } = null!;
        public string TenSp { get; set; } = null!;

        public string? MaDanhMuc { get; set; }

        public decimal Gia { get; set; }

        public string? MoTa { get; set; }

        public int? SoLuongTon { get; set; }

        public decimal? TheTich { get; set; }

        public string? DonVi { get; set; }

        public List<IFormFile>? AnhSps { get; set; }
    }
}
