using DACN_H_P.Dtos.Response;
using DACN_H_P.Model;

namespace DACN_H_P.Mapper
{
    public class ChiTietKhuyenMaiBuilder
    {
        public static ChiTietKhuyenMaiResponse ToResponse(ChiTietKm ctkm)
        {
            return new ChiTietKhuyenMaiResponse
            {
                MaKhuyenMai = ctkm.MaKhuyenMai,
                MaSp = ctkm.MaSp,
                TenSP = ctkm.MaSpNavigation?.TenSp ?? "N/a",
                PhanTramGiam = ctkm.PhanTramGiam
            };
        }
    }
}
