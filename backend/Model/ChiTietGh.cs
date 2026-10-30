using System;
using System.Collections.Generic;

namespace DACN_H_P.Model;

public partial class ChiTietGh
{
    public string MaGioHang { get; set; } = null!;

    public string MaSp { get; set; } = null!;

    public int SoLuong { get; set; }

    public virtual GioHang MaGioHangNavigation { get; set; } = null!;

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
