using DACN_H_P.Model;
using Microsoft.EntityFrameworkCore;

namespace DACN_H_P.Repository.Impl
{
    public class TaiKhoanRepository:ITaiKhoanRepository
    {
        private readonly DacnHContext _context;

        public TaiKhoanRepository(DacnHContext context)
        {
            _context = context;
        }

        public async Task<TaiKhoan?> GetByTenDangNhapAsync(string username)
        {
            return await _context.TaiKhoans.FirstOrDefaultAsync(tk => tk.TenDangNhap == username);
        }
        public async Task<List<TaiKhoan>> GetAllTaiKhoan()
        {
            return await _context.TaiKhoans.AsNoTracking().ToListAsync();
        }
        public async Task<TaiKhoan?> GetByMaTaiKhoan(string maTaiKhoan)
        {
            var exist = await _context.TaiKhoans.FindAsync(maTaiKhoan);
            return exist;
        }
        public async Task AddAsync(TaiKhoan tk)
        {
            await _context.TaiKhoans.AddAsync(tk);
        }
        public async Task SavechangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(TaiKhoan tk)
        {
            try
            {
                _context.TaiKhoans.Update(tk);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteOrLockTaiKhoan(string matk)
        {
            try
            {
                var tk = await _context.TaiKhoans
                    .Include(t => t.DonHangs)
                    .FirstOrDefaultAsync(t => t.MaTaiKhoan == matk);

                if (tk == null) return false;

                if (tk.DonHangs.Any())
                {
                    return false;
                }

                _context.TaiKhoans.Remove(tk);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
