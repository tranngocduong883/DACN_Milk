namespace DACN_H_P.Dtos.Response
{
    public class SanPhamResponse
    {
        public string MaSp { get; set; } = null!;
        public string TenSp { get; set; } = null!;
        public string? MaDanhMuc { get; set; }
        public string? TenDanhMuc { get; set; }

        public decimal Gia { get; set; }
        public decimal GiaKhuyenMai { get; set; }

        public string? MoTa { get; set; }

        public int? SoLuongTon { get; set; }

        public decimal? TheTich { get; set; }

        public string? DonVi { get; set; }
        public string? AnhDaiDien { get; set; }
        public string? AnhPhu1 { get; set; }
        public string? AnhPhu2 { get; set; }

        public List<string> DuongDanAnhSPs { get; set; } = new List<string>();
    }
}
