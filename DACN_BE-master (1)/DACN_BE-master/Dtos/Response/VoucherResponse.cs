namespace DACN_H_P.Dtos.Response
{
    public class VoucherResponse
    {
        public string MaVoucher { get; set; } = null!;
        public string TenVoucher { get; set; } = null!;
        public decimal GiamGia { get; set; }
        public decimal DieuKienApDung { get; set; }
        public string MoTa { get; set; } = null!;
        public DateTime NgayBatDau {  get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string TrangThai { get; set; } = null!;
    }
}
