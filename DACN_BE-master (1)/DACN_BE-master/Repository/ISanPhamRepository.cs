using DACN_H_P.Dtos.Response;
using DACN_H_P.Model;

namespace DACN_H_P.Repository
{
    public interface ISanPhamRepository
    {
        Task<IEnumerable<SanPham>> GetAllAsync();
        Task<SanPham?> GetByMaSP(string masp);
        Task<IEnumerable<SanPham?>> GetByTenSP(string tensp);
        Task<IEnumerable<SanPham?>> GetByMaDM(string madm);
        Task<SanPham?> PostAsync(SanPham sanPham);
        Task<SanPham?> PutAsync(SanPham sanPham);
        Task<bool> DeleteAsync(string masp);

        Task<SanPham?> GetFullInfoByMaSP(string masp);
    }
}
