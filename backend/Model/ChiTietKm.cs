using System;
using System.Collections.Generic;

namespace DACN_H_P.Model;

public partial class ChiTietKm
{
    public string MaKhuyenMai { get; set; } = null!;

    public string MaSp { get; set; } = null!;

    public int PhanTramGiam { get; set; }

    public virtual KhuyenMai MaKhuyenMaiNavigation { get; set; } = null!;

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
