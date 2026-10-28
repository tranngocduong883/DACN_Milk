using System;
using System.Collections.Generic;

namespace DACN_H_P.Model;

public partial class AnhSp
{
    public string MaAnh { get; set; } = null!;

    public string MaSp { get; set; } = null!;

    public string DuongDan { get; set; } = null!;

    public virtual SanPham MaSpNavigation { get; set; } = null!;
}
