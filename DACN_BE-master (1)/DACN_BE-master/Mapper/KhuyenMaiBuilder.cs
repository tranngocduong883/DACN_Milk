using DACN_H_P.Dtos.Response;
using DACN_H_P.Model;

namespace DACN_H_P.Mapper
{
    public class KhuyenMaiBuilder
    {
        public static KhuyenMaiResponse ToResponse(KhuyenMai km)
        {
            string statusKM = string.Empty;
            if (km.NgayBatDau > DateTime.Now)
            {
                statusKM = "Chưa bắt đầu";
            }
            else if(km.NgayBatDau <= DateTime.Now && km.NgayKetThuc >= DateTime.Now)
            {
                statusKM = "Còn hiệu lực";
            }
            else
            {
                statusKM = "Hết hạn";
            }
            return new KhuyenMaiResponse
            {
                MaKhuyenMai = km.MaKhuyenMai,
                TenKhuyenMai = km.TenKhuyenMai,
                NgayBatDau = km.NgayBatDau,
                NgayKetThuc = km.NgayKetThuc,
                MoTa = km.MoTa,
                TrangThai = statusKM
            };
        }
    }
}
