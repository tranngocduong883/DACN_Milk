namespace DACN_H_P.Dtos.Request
{
    public class DatHangRequest
    {
        public string MaTaiKhoan { get; set; } = null!;
        public string MaPTTT { get; set; } = null!;
        public string MaPTVC { get; set; } = null!;
        public string? MaVoucher { get; set; }
        public string DiaChiGiaoHang { get; set; } = null!;
    }
}
