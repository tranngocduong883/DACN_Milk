namespace DACN_H_P.Dtos.Response
{
    public class KhuyenMaiResponse
    {
        public string MaKhuyenMai { get; set; } = null!;
        public string TenKhuyenMai { get; set; } = null!;
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string? MoTa { get; set; } = null!;
        public string TrangThai { get; set; } = string.Empty;
    }
}
