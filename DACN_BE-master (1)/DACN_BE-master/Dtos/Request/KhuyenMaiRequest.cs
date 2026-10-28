namespace DACN_H_P.Dtos.Request
{
    public class KhuyenMaiRequest
    {
        public string MaKhuyenMai { get; set; } = null!;

        public string TenKhuyenMai { get; set; } = null!;

        public DateTime NgayBatDau { get; set; }

        public DateTime NgayKetThuc { get; set; }

        public string? MoTa { get; set; }
    }
}
