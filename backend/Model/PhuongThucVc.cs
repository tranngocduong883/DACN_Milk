using System;
using System.Collections.Generic;

namespace DACN_H_P.Model;

public partial class PhuongThucVc
{
    public string MaPtvc { get; set; } = null!;

    public string TenPtvc { get; set; } = null!;

    public decimal PhiVanChuyen { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
