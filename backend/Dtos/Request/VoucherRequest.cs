using System.ComponentModel.DataAnnotations;

namespace DACN_H_P.Dtos.Request
{
    public class VoucherRequest : IValidatableObject
    {
        [Required(ErrorMessage = "Mã voucher không được để trống")]
        public string MaVoucher { get; set; } = null!;

        public string? TenVoucher { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Giá trị giảm phải lớn hơn 0")]
        public decimal GiamGia { get; set; }

        public DateTime NgayBatDau { get; set; }

        public DateTime NgayKetThuc { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Điều kiện áp dụng không được âm")]
        public decimal? DieuKienApDung { get; set; }

        public string? MoTa { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NgayKetThuc < NgayBatDau)
            {
                yield return new ValidationResult("Ngày kết thúc phải sau ngày bắt đầu", new[] { nameof(NgayKetThuc) });
            }
        }
    }
}
