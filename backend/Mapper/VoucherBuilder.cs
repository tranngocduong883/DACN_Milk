using DACN_H_P.Dtos.Response;
using DACN_H_P.Model;

namespace DACN_H_P.Mapper
{
    public class VoucherBuilder
    {
        public static VoucherResponse ToResponse(Voucher v)
        {
            string statusVoucher = string.Empty;
            if(v.NgayBatDau > DateTime.Now)
            {
                statusVoucher = "Sắp được dùng";
            }
            else if(v.NgayBatDau <= DateTime.Now && v.NgayKetThuc >= DateTime.Now)
            {
                statusVoucher = "Được sử dụng";
            }
            else
            {
                statusVoucher = "Hết hạn";
            }
            return new VoucherResponse
            {
                MaVoucher = v.MaVoucher,
                TenVoucher = v.TenVoucher ?? "Voucher giảm giá",
                GiamGia = v.GiamGia,
                DieuKienApDung = v.DieuKienApDung ?? 0,
                MoTa = v.MoTa ?? $"Giảm {v.GiamGia:N0}đ cho đơn từ {v.DieuKienApDung:N0}đ",
                NgayBatDau = v.NgayBatDau,
                NgayKetThuc = v.NgayKetThuc,
                TrangThai = statusVoucher
            };
        }
    }
}
