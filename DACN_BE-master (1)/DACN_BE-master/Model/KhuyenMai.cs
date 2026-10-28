using System;
using System.Collections.Generic;

namespace DACN_H_P.Model;

public partial class KhuyenMai
{
    public string MaKhuyenMai { get; set; } = null!;

    public string TenKhuyenMai { get; set; } = null!;

    public DateTime NgayBatDau { get; set; }

    public DateTime NgayKetThuc { get; set; }

    public string? MoTa { get; set; }

    public virtual ICollection<ChiTietKm> ChiTietKms { get; set; } = new List<ChiTietKm>();
}
