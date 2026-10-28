using DACN_H_P.Dtos.Response;
using DACN_H_P.Model;

namespace DACN_H_P.Mapper
{
    public class CuaHangBuilder
    {
        public static CuaHangResponse ToResponse(CuaHang ch)
        {
            return new CuaHangResponse
            {
                MaCuaHang = ch.MaCuaHang,
                TenCuaHang = ch.TenCuaHang,
                DiaChi = ch.DiaChi,
                SoDienThoai = ch.SoDienThoai,
                Email = ch.Email,
                MoTa = ch.MoTa
            };
        }
    }
}
