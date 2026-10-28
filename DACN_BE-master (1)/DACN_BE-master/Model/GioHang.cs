using System;
using System.Collections.Generic;

namespace DACN_H_P.Model;

public partial class GioHang
{
    public string MaGioHang { get; set; } = null!;

    public string? MaTaiKhoan { get; set; }

    public virtual ICollection<ChiTietGh> ChiTietGhs { get; set; } = new List<ChiTietGh>();

    public virtual TaiKhoan? MaTaiKhoanNavigation { get; set; }
}
