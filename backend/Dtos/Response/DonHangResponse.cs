namespace DACN_H_P.Dtos.Response
{
    public class DonHangResponse
    {
        public string MaDonHang { get; set; } = null!;
        public string MaTaiKhoan { get; set; } = null!;
        public string? TenNguoiDat { get; set; }
        public DateTime NgayDatHang { get; set; }
        public string TrangThaiDonHang { get; set; } = null!;
        public decimal TongTienHang { get; set; }
        public string DiaChiGiaoHang { get; set; } = null!;
        public string PhuongThucThanhToan { get; set; } = null!;
        public string PhuongThucVanChuyen { get; set; } = null!;
        public decimal? PhiShip { get; set; }
        public decimal? GiamGiaVoucher { get; set; }
        public DateTime NgayGiaoHang { get; set; }
        public decimal ThanhToan { get; set; }
        public List<ChiTietDonHangResponse> ChiTietDonHang { get; set; } = new();
    }
}
