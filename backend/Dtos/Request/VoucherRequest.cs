namespace DACN_H_P.Dtos.Request
{
    public class VoucherRequest
    {
        public string MaVoucher { get; set; } = null!;

        public string? TenVoucher { get; set; }

        public decimal GiamGia { get; set; }

        public DateTime NgayBatDau { get; set; }

        public DateTime NgayKetThuc { get; set; }

        public decimal? DieuKienApDung { get; set; }

        public string? MoTa { get; set; }
    }
}
