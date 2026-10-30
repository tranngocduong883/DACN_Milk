using System;
using System.Collections.Generic;

namespace DACN_H_P.Model;

public partial class DonHang
{
    public string MaDonHang { get; set; } = null!;

    public string? MaPttt { get; set; }

    public string? MaVoucher { get; set; }

    public string MaTaiKhoan { get; set; } = null!;

    public string? MaPtvc { get; set; }

    public decimal TongTienHang { get; set; }

    public string? TrangThaiDonHang { get; set; }

    public DateTime? NgayDatHang { get; set; }

    public string DiaChiGh { get; set; } = null!;

    public virtual ICollection<ChiTietDh> ChiTietDhs { get; set; } = new List<ChiTietDh>();

    public virtual PhuongThucTt? MaPtttNavigation { get; set; }

    public virtual PhuongThucVc? MaPtvcNavigation { get; set; }

    public virtual TaiKhoan MaTaiKhoanNavigation { get; set; } = null!;

    public virtual Voucher? MaVoucherNavigation { get; set; }
}
