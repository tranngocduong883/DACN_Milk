using DACN_H_P.Model;

namespace DACN_H_P.Repository
{
    public interface ITaiKhoanRepository
    {
        Task<TaiKhoan?> GetByTenDangNhapAsync(string tenDangNhap);
        Task AddAsync(TaiKhoan taiKhoan);
        Task SavechangeAsync();

        Task<TaiKhoan?> GetByMaTaiKhoan(string maTaiKhoan);
        Task<List<TaiKhoan>> GetAllTaiKhoan();
        Task<bool> UpdateAsync(TaiKhoan tk);
        Task<bool> DeleteOrLockTaiKhoan(string matk);
    }
}
