using DACN_H_P.Dtos.Response;
using DACN_H_P.Dtos.Request;

namespace DACN_H_P.Service
{
    public interface IGioHangService
    {
        Task<(bool success, string message,GioHangResponse response)> GetGioHangByMaTaiKhoanAsync(string matk);
        Task<(bool success, string message,GioHangResponse response)> AddOrUpdateGioHang(string matk, ChiTietGioHangRequest request);
        Task<(bool success, string message, GioHangResponse response)> DecreaseItemGioHang(string matk, ChiTietGioHangRequest request);
        Task<(bool success, string message, GioHangResponse response)> UpdateItemGioHang(string matk, ChiTietGioHangRequest request);
        Task<(bool success, string message, GioHangResponse response)> RemoveItemAsync(string matk, string masp);
        Task<(bool success, string message, GioHangResponse response)> RemoveAllGioHang(string matk);
    }
}
