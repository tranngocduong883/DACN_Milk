using DACN_H_P.Dtos.Response;
using DACN_H_P.Model;

namespace DACN_H_P.Mapper
{
    public class ChiTietDonHangBuilder
    {
        public static ChiTietDonHangResponse ToResponse(ChiTietDh ct)
        {
            return new ChiTietDonHangResponse
            {
                MaSP = ct.MaSp,
                TenSP = ct.MaSpNavigation.TenSp,
                SoLuong = ct.SoLuong,
                DonGia = ct.DonGiaLucDat
            };
        }
    }
}
