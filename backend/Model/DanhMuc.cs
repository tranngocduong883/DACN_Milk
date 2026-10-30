using System;
using System.Collections.Generic;

namespace DACN_H_P.Model;

public partial class DanhMuc
{
    public string MaDanhMuc { get; set; } = null!;

    public string TenDanhMuc { get; set; } = null!;

    public string? MoTa { get; set; }

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
