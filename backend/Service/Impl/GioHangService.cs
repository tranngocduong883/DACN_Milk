using DACN_H_P.Dtos.Request;
using DACN_H_P.Dtos.Response;
using DACN_H_P.Mapper;
using DACN_H_P.Repository;

namespace DACN_H_P.Service.Impl
{
    public class GioHangService : IGioHangService
    {
        private readonly IGioHangRepository _repo;

        public GioHangService(IGioHangRepository repo)
        {
            _repo = repo;
        }
        public async Task<(bool, string, GioHangResponse)> GetGioHangByMaTaiKhoanAsync(string matk)
        {
            var gioHang = await _repo.GetGioHangByMaTaiKhoanAsync(matk);
            if (gioHang == null)
            {
                return (false, "Không có tài khoản này", null);
            }
            return (true, "Lấy thành công giỏ hàng", GioHangBuilder.ToResponse(gioHang));
        }

        public async Task<(bool, string, GioHangResponse)> AddOrUpdateGioHang(string matk, ChiTietGioHangRequest request)
        {
            var result = await _repo.AddOrUpdateGioHang(matk, request.MaSP, request.SoLuong);
            if (!result)
            {
                return (false, "Tài khoản không tồn tại", null);
            }
            await _repo.SavechangeAsync();
            var gioHang = await _repo.GetGioHangByMaTaiKhoanAsync(matk);
            var response = GioHangBuilder.ToResponse(gioHang);
            return (true, "Thêm hoặc cập nhật giỏ hàng thành công", response);
        }
        public async Task<(bool, string, GioHangResponse)> DecreaseItemGioHang(string matk, ChiTietGioHangRequest request)
        {
            var result = await _repo.DecreaseItemGioHang(matk, request.MaSP, request.SoLuong);
            if (!result)
            {
                return (false, "Tài khoản không tồn tại", null);
            }
            await _repo.SavechangeAsync();
            var gioHang = await _repo.GetGioHangByMaTaiKhoanAsync(matk);
            var response = GioHangBuilder.ToResponse(gioHang);
            return (true, "Giảm số lượng sản phẩm trong giỏ hàng thành công", response);
        }
        public async Task<(bool, string, GioHangResponse)> UpdateItemGioHang(string matk, ChiTietGioHangRequest request)
        {
            var result = await _repo.UpdateItemGioHang(matk, request.MaSP, request.SoLuong);
            if (!result)
            {
                return (false, "Tài khoản không tồn tại", null);
            }
            await _repo.SavechangeAsync();
            var gioHang = await _repo.GetGioHangByMaTaiKhoanAsync(matk);
            var response = GioHangBuilder.ToResponse(gioHang);
            return (true, "Cập nhật số lượng sản phẩm trong giỏ hàng thành công", response);
        }
        public async Task<(bool, string, GioHangResponse)> RemoveItemAsync(string matk, string masp)
        {
            var result = await _repo.RemoveItemAsync(matk, masp);
            if (!result)
            {
                return (false, "Tài khoản không tồn tại", null);
            }
            await _repo.SavechangeAsync();
            var gioHang = await _repo.GetGioHangByMaTaiKhoanAsync(matk);
            var response = GioHangBuilder.ToResponse(gioHang);
            return (true, "Xóa sản phẩm khỏi giỏ hàng thành công", response);
        }
        public async Task<(bool, string, GioHangResponse)> RemoveAllGioHang(string matk)
        {
            var result = await _repo.RemoveAllGioHang(matk);
            if (!result)
            {
                return (false, "Tài khoản không tồn tại", null);
            }
            await _repo.SavechangeAsync();
            var gioHang = await _repo.GetGioHangByMaTaiKhoanAsync(matk);
            var response = GioHangBuilder.ToResponse(gioHang);
            return (true, "Xóa tất cả sản phẩm khỏi giỏ hàng thành công", response);
        }
    }
}
