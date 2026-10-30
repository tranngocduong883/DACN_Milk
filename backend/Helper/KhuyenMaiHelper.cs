using DACN_H_P.Model;
using Microsoft.Extensions.Logging.Abstractions;

namespace DACN_H_P.Helper
{
    public class KhuyenMaiHelper
    {
        public static decimal TinhGiaKhuyenMai(SanPham sp)
        {
            if(sp.ChiTietKms == null || !sp.ChiTietKms.Any())
            {
                return sp.Gia;
            }
            var now = DateTime.Now;
            var kmTotNhat = sp.ChiTietKms.Where(ctkm => ctkm.MaKhuyenMaiNavigation != null
                                            && ctkm.MaKhuyenMaiNavigation.NgayBatDau <= now
                                            && ctkm.MaKhuyenMaiNavigation.NgayKetThuc >= now)
                                            .OrderByDescending(ct => ct.PhanTramGiam)
                                            .FirstOrDefault();
            if(kmTotNhat == null)
            {
                return sp.Gia;
            }
            else
            {
                decimal giakm;
                giakm = sp.Gia - (sp.Gia * kmTotNhat.PhanTramGiam / 100);
                return giakm;
            }
        } 
    }
}
