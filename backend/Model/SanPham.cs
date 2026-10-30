using System;
using System.Collections.Generic;

namespace DACN_H_P.Model;

public partial class SanPham
{
    public string MaSp { get; set; } = null!;

    public string? MaDanhMuc { get; set; }

    public decimal Gia { get; set; }

    public string? MoTa { get; set; }

    public int? SoLuongTon { get; set; }

    public decimal? TheTich { get; set; }

    public string? DonVi { get; set; }

    public string TenSp { get; set; } = null!;

    public virtual ICollection<AnhSp> AnhSps { get; set; } = new List<AnhSp>();

    public virtual ICollection<ChiTietDh> ChiTietDhs { get; set; } = new List<ChiTietDh>();

    public virtual ICollection<ChiTietGh> ChiTietGhs { get; set; } = new List<ChiTietGh>();

    public virtual ICollection<ChiTietKm> ChiTietKms { get; set; } = new List<ChiTietKm>();

    public virtual ICollection<DanhGiaSp> DanhGiaSps { get; set; } = new List<DanhGiaSp>();

    public virtual DanhMuc? MaDanhMucNavigation { get; set; }
}
