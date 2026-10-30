using DACN_H_P.Dtos.Request;
using DACN_H_P.Dtos.Response;
using DACN_H_P.Helper;
using DACN_H_P.Mapper;
using DACN_H_P.Model;
using DACN_H_P.Repository;
using DACN_H_P.Utils;

namespace DACN_H_P.Service.Impl
{
    public class DonHangService : IDonHangService
    {
        private readonly IDonHangRepositoy _repo;
        private readonly IGioHangRepository _gioRepo;
        private readonly IVoucherRepository _voucherRepo;

        public DonHangService(IDonHangRepositoy repo, IGioHangRepository gioRepo, IVoucherRepository voucherRepo)
        {
            _repo = repo;
            _gioRepo = gioRepo;
            _voucherRepo = voucherRepo;
        }
        public async Task<(bool, string, DonHangResponse?)> CreateDonHang(DatHangRequest request)
        {
            if (!await _repo.CheckPTTT(request.MaPTTT))
            {
                return (false, "Phương thức thanh toán không hợp lệ", null);
            }
            if (!await _repo.CheckPTVC(request.MaPTVC))
            {
                return (false, "Phương thức vận chuyển không hợp lệ", null);
            }
            if(!await _repo.CheckAcc(request.MaTaiKhoan))
            {
                return (false, "Tài khoản không tồn tại", null);
            }
            var gioHang = await _gioRepo.GetGioHangByMaTaiKhoanAsync(request.MaTaiKhoan);
            if (gioHang == null || !gioHang.ChiTietGhs.Any())
            {
                return (false, "Giỏ hàng trống", null);
            }
            var donHang = new DonHang
            {
                MaDonHang = Guid.NewGuid().ToString(),
                MaTaiKhoan = request.MaTaiKhoan,
                NgayDatHang = DateTime.Now,
                TrangThaiDonHang = TrangThaiDonHang.ChoXacNhan,

                MaPttt = request.MaPTTT,
                MaPtvc = request.MaPTVC,
                DiaChiGh = request.DiaChiGiaoHang,

                MaVoucher = null,

                ChiTietDhs = new List<ChiTietDh>()
            };
            decimal tongTienHang = 0;

            foreach (var item in gioHang.ChiTietGhs)
            {
                var sp = item.MaSpNavigation;

                decimal donGiaLucDat = KhuyenMaiHelper.TinhGiaKhuyenMai(sp);

                tongTienHang += donGiaLucDat * item.SoLuong;

                donHang.ChiTietDhs.Add(new ChiTietDh
                {
                    MaDonHang = donHang.MaDonHang,
                    MaSp = item.MaSp,
                    SoLuong = item.SoLuong,
                    DonGiaLucDat = donGiaLucDat
                });
            }
            donHang.TongTienHang = tongTienHang;
            if (!string.IsNullOrEmpty(request.MaVoucher))
            {
                var voucher = await _voucherRepo.GetVoucherById(request.MaVoucher);
                var now = DateTime.Now;

                if (voucher == null || voucher.NgayBatDau > now || voucher.NgayKetThuc < now)
                {
                    return (false, "Voucher không tồn tại hoặc đã hết hạn", null);
                }

                decimal dieuKienMin = voucher.DieuKienApDung ?? 0;
                if (tongTienHang < dieuKienMin)
                {
                    return (false, $"Đơn hàng chưa đủ điều kiện. Cần tối thiểu {dieuKienMin:N0}đ", null);
                }

                var usedCodes = await _voucherRepo.GetUsedVoucherCodes(request.MaTaiKhoan);
                if (usedCodes.Contains(request.MaVoucher))
                {
                    return (false, "Bạn đã sử dụng mã giảm giá này rồi", null);
                }

                donHang.MaVoucher = request.MaVoucher;
            }
            await _repo.CreateDonHang(donHang);
            await _gioRepo.RemoveAllGioHang(request.MaTaiKhoan);
            var donhangInfo = await _repo.GetDonHangByMaDH(donHang.MaDonHang);
            if (donhangInfo == null)
            {
                return (true, "Đặt hàng thành công nhưng có lỗi hiển thị", null);
            }
            return (true, "Đặt hàng thành công", DonHangBuilder.ToResponse(donhangInfo));
        }
        public async Task<DonHangResponse?> GetDonHangByMaDH(string maDH)
        {
            var result = await _repo.GetDonHangByMaDH(maDH);
            return result == null ? null : DonHangBuilder.ToResponse(result);
        }
        public async Task<IEnumerable<DonHangResponse>?> GetDonHangByMaTK(string maTK)
        {
            var checkAcc = await _repo.CheckAcc(maTK);
            if (!checkAcc)
            {
                return null;
            }
            var result = await _repo.GetDonHangByMaTK(maTK);
            return result.Select(DonHangBuilder.ToResponse).ToList();
        }

        public async Task<IEnumerable<DonHangResponse>> GetAllDonHang()
        {
            var result = await _repo.GetAllDonHang();
            return result.Select(DonHangBuilder.ToResponse).ToList();
        }

        public async Task<(bool, string)> DeleteDonHang(string maDH)
        {
            var dh = await _repo.GetDonHangByMaDH(maDH);
            if(dh == null)
            {
                return (false, "Không có đơn hàng này");
            }
            if(dh.TrangThaiDonHang == TrangThaiDonHang.GiaoHangThanhCong || dh.TrangThaiDonHang == TrangThaiDonHang.ChoXacNhan || dh.TrangThaiDonHang == TrangThaiDonHang.DangVanChuyen)
            {
                return (false, "Không được xóa đơn hàng này do nghiệp vụ không cho phép");
            }
            var result = await _repo.DeleteDonHang(maDH);
            if (!result)
            {
                return (false, "Có lỗi khi xóa");
            }
            return (true, "Xóa thành công đơn hàng");
        }
        public async Task<(bool, string, DonHangResponse?)> UpdateTrangThaiDonHang(string maDonHang, string trangThaiMoi)
        {
            var donHang = await _repo.GetDonHangByMaDH(maDonHang);
            if (donHang == null) return (false, "Đơn hàng không tồn tại", null);

            string trangThaiCu = donHang.TrangThaiDonHang;
            if (trangThaiCu == trangThaiMoi)
            {
                return (true, "Trạng thái đã được cập nhật trước đó rồi", DonHangBuilder.ToResponse(donHang));
            }

            bool luongTrangThai = false;
            switch (trangThaiCu)
            {
                case TrangThaiDonHang.ChoXacNhan:
                    if (trangThaiMoi == TrangThaiDonHang.DangVanChuyen || trangThaiMoi == TrangThaiDonHang.DaHuy)
                    {
                        luongTrangThai = true;
                    }
                    break;

                case TrangThaiDonHang.DangVanChuyen:
                    if(trangThaiMoi == TrangThaiDonHang.GiaoHangThanhCong || trangThaiMoi == TrangThaiDonHang.DaHuy)
                    {
                        luongTrangThai = true;
                    }
                    break;
                case TrangThaiDonHang.GiaoHangThanhCong:
                case TrangThaiDonHang.DaHuy:
                    return (false, "Đơn hàng đã hoàn tất hoặc đã hủy, không thể thay đổi trạng thái nữa", DonHangBuilder.ToResponse(donHang));

                default:
                    return (false, "Trạng thái đơn hàng hiện tại không hợp lệ(data bug)", null);

            }

            if (!luongTrangThai)
            {
                return (false, $"Không thể chuyển trạng thái từ '{trangThaiCu}' sang '{trangThaiMoi}'. Quy trình không hợp lệ!", DonHangBuilder.ToResponse(donHang));
            }

            donHang.TrangThaiDonHang = trangThaiMoi;


            await _repo.UpdateDonHang(donHang);

            return (true, $"Cập nhật trạng thái thành {trangThaiMoi}",DonHangBuilder.ToResponse(donHang));
        }
        public async Task<(bool, string, DonHangResponse?)> HuyDonHang(string maDonHang, string maTaiKhoan)
        {
            var donHang = await _repo.GetDonHangByMaDH(maDonHang);
            if (donHang == null) return (false, "Đơn hàng không tồn tại", null);

            if (donHang.MaTaiKhoan != maTaiKhoan) return (false, "Bạn không có quyền hủy đơn này", null);

            if (donHang.TrangThaiDonHang != TrangThaiDonHang.ChoXacNhan)
            {
                return (false, "Đơn hàng đang vận chuyển hoặc đã xong, không thể hủy", DonHangBuilder.ToResponse(donHang));
            }

            donHang.TrangThaiDonHang = TrangThaiDonHang.DaHuy;

            await _repo.UpdateDonHang(donHang);
            return (true, "Đã hủy đơn hàng thành công", DonHangBuilder.ToResponse(donHang));
        }
        public async Task<(bool, string, DonHangResponse?)> UpdateDiaChiGiaoHang(string maDonHang, string maTaiKhoan, string diaChiMoi)
        {
            var donHang = await _repo.GetDonHangByMaDH(maDonHang);
            if (donHang == null) return (false, "Đơn hàng không tồn tại", null);

            if (donHang.MaTaiKhoan != maTaiKhoan) return (false, "Không đúng tài khoản", null);

            if (donHang.TrangThaiDonHang != TrangThaiDonHang.ChoXacNhan)
            {
                return (false, "Đơn hàng đã đi giao, không thể thay đổi địa chỉ", DonHangBuilder.ToResponse(donHang));
            }

            donHang.DiaChiGh = diaChiMoi;
            await _repo.UpdateDonHang(donHang);

            return (true, "Cập nhật địa chỉ thành công", DonHangBuilder.ToResponse(donHang));
        }
    }

}
