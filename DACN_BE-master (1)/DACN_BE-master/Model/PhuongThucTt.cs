using System;
using System.Collections.Generic;

namespace DACN_H_P.Model;

public partial class PhuongThucTt
{
    public string MaPttt { get; set; } = null!;

    public string TenPttt { get; set; } = null!;

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
