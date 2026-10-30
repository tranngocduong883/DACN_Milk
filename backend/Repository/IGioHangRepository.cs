using DACN_H_P.Dtos.Response;
using DACN_H_P.Model;

namespace DACN_H_P.Repository
{
    public interface IGioHangRepository
    {
        Task<GioHang?> GetGioHangByMaTaiKhoanAsync(string matk);
        Task<bool> AddOrUpdateGioHang(string matk, string masp, int soluong);
        Task<bool> DecreaseItemGioHang(string matk, string masp, int soluong);
        Task<bool> UpdateItemGioHang(string matk, string masp, int soluong);
        Task<bool> RemoveItemAsync(string matk, string masp);
        Task<bool> RemoveAllGioHang(string matk);
        Task SavechangeAsync();
    }
}
