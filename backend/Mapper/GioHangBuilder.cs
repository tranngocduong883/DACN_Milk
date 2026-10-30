using DACN_H_P.Dtos.Response;
using DACN_H_P.Model;

namespace DACN_H_P.Mapper
{
    public class GioHangBuilder
    {
        public static GioHangResponse ToResponse(GioHang gh)
        {
            return new GioHangResponse
            {
                MaGioHang = gh.MaGioHang,
                MaTaiKhoan = gh.MaTaiKhoan,
                SanPhams = gh.ChiTietGhs.Select(ct => ChiTietGioHangBuilder.ToResponse(ct, ct.MaSpNavigation)).ToList()
            };
        }
    }
}
