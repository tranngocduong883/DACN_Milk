namespace DACN_H_P.Dtos.Response
{
    public class ChiTietDonHangResponse
    {
        public string MaSP { get; set; } = null!;
        public string TenSP { get; set; } = null!;
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
    }
}
