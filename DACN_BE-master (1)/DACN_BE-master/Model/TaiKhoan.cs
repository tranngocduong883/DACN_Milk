using System;
using System.Collections.Generic;

namespace DACN_H_P.Model;

public partial class TaiKhoan
{
    public string MaTaiKhoan { get; set; } = null!;

    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string? Email { get; set; }

    public string? HoTen { get; set; }

    public string? SoDienThoai { get; set; }

    public string? VaiTro { get; set; }

    public string? DiaChi { get; set; }

    public virtual ICollection<DanhGiaSp> DanhGiaSps { get; set; } = new List<DanhGiaSp>();

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual GioHang? GioHang { get; set; }
}
