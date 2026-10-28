namespace DACN_H_P.Dtos.Response
{
    public class GioHangResponse
    {
        public string MaGioHang { get; set; } = null!;
        public string? MaTaiKhoan { get; set; } = null;
        public List<ChiTietGHResponse> SanPhams { get; set; } = new();
        public decimal TongTien
        {
            get
            {
                return SanPhams.Sum(sp => sp.ThanhTien);
            }
        }
    }
}
