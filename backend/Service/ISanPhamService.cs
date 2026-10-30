using DACN_H_P.Dtos.Request;
using DACN_H_P.Dtos.Response;

namespace DACN_H_P.Service
{
    public interface ISanPhamService
    {
        Task<IEnumerable<SanPhamResponse>> GetAllAsync();
        Task<SanPhamResponse?> GetByMaSP(string masp);
        Task<IEnumerable<SanPhamResponse?>> GetByTenSP(string tensp);
        Task<IEnumerable<SanPhamResponse?>> GetByMaDM(string madm);
        Task<(bool success, string message, SanPhamResponse response)> PostAsync(SanPhamRequest sanPhamRequest);
        Task<(bool success, string message, SanPhamResponse response)> PutAsync(SanPhamRequest sanPhamRequest);
        Task<(bool success, string message)> DeleteAsync(string masp);
    }
}
