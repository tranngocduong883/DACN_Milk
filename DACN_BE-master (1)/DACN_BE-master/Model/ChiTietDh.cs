using System;
using System.Collections.Generic;

namespace DACN_H_P.Model;

public partial class ChiTietDh
{
    public string MaDonHang { get; set; } = null!;

    public string MaSp { get; set; } = null!;

    public int SoLuong { get; set; }

    public decimal DonGiaLucDat { get; set; }

    public virtual DonHang MaDonHangNavigation { get; set; } = null!;

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
