using DACN_H_P.Dtos.Response;
using DACN_H_P.Model;

namespace DACN_H_P.Mapper
{
    public class DanhMucBuilder
    {
        public static DanhMucResponse ToResponse(DanhMuc danhmuc)
        {
            return new DanhMucResponse
            {
                MaDanhMuc = danhmuc.MaDanhMuc,
                TenDanhMuc = danhmuc.TenDanhMuc,
                MoTa = danhmuc.MoTa
            };
        }
    }
}
