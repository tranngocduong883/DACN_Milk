using DACN_H_P.Dtos.Response;
using DACN_H_P.Helper;
using DACN_H_P.Model;

namespace DACN_H_P.Mapper
{
    public class SanPhamBuilder
    {
        public static SanPhamResponse ToResponse(SanPham sp)
        {
            var giakhuyenmai = KhuyenMaiHelper.TinhGiaKhuyenMai(sp);
            return new SanPhamResponse
            {
                MaSp = sp.MaSp,
                TenSp = sp.TenSp,
                MaDanhMuc = sp.MaDanhMuc,
                TenDanhMuc = sp.MaDanhMucNavigation?.TenDanhMuc ?? "N/a",
                Gia = sp.Gia,
                GiaKhuyenMai = giakhuyenmai,
                MoTa = sp.MoTa,
                SoLuongTon = sp.SoLuongTon,
                TheTich = sp.TheTich,
                DonVi = sp.DonVi,
                AnhDaiDien = sp.AnhSps.FirstOrDefault()?.DuongDan,
                AnhPhu1 = sp.AnhSps.Skip(1).FirstOrDefault()?.DuongDan,
                AnhPhu2 = sp.AnhSps.Skip(2).FirstOrDefault()?.DuongDan,
                DuongDanAnhSPs = sp.AnhSps.Select(a => a.DuongDan).ToList()
            };
        }
    }
}
