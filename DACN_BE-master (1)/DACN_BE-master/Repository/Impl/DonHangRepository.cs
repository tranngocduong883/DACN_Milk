using DACN_H_P.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DACN_H_P.Utils;

namespace DACN_H_P.Repository.Impl
{
    public class DonHangRepository : IDonHangRepositoy
    {
        private readonly DacnHContext _context;
        public DonHangRepository(DacnHContext context)
        {
            _context = context;
        }

        public async Task<DonHang> CreateDonHang(DonHang donHang)
        {
            _context.DonHangs.Add(donHang);
            await _context.SaveChangesAsync();
            return donHang;
        }
        public async Task<List<ChiTietDh>> CreateChiTietDonHang(List<ChiTietDh> cts)
        {
            await _context.ChiTietDhs.AddRangeAsync(cts);
            await _context.SaveChangesAsync();
            return cts;
        }
        public async Task<DonHang?> GetDonHangByMaDH(string maDH)
        {
            return await _context.DonHangs
                .Include(c => c.ChiTietDhs).ThenInclude(m => m.MaSpNavigation)
                .Include(v => v.MaVoucherNavigation)
                .Include(ptvc => ptvc.MaPtvcNavigation)
                .Include(pttt => pttt.MaPtttNavigation)
                .FirstOrDefaultAsync(d => d.MaDonHang == maDH);
        }
        public async Task<List<DonHang>> GetDonHangByMaTK(string matk)
        {
            return await _context.DonHangs
                .AsNoTracking()
                .Include(c => c.ChiTietDhs).ThenInclude(m => m.MaSpNavigation)
                .Include(v => v.MaVoucherNavigation)
                .Include(ptvc => ptvc.MaPtvcNavigation)
                .Include(pttt => pttt.MaPtttNavigation)
                .Where(tk => tk.MaTaiKhoan == matk)
                .OrderByDescending(d => d.NgayDatHang)
                .ToListAsync();
        }
        public async Task<List<DonHang>> GetAllDonHang()
        {
            return await _context.DonHangs
                .AsNoTracking()
                .Include(c => c.ChiTietDhs).ThenInclude(m => m.MaSpNavigation)
                .Include(v => v.MaVoucherNavigation)
                .Include(ptvc => ptvc.MaPtvcNavigation)
                .Include(pttt => pttt.MaPtttNavigation)
                .Include(tk => tk.MaTaiKhoanNavigation)
                .OrderByDescending(d => d.NgayDatHang)
                .ToListAsync();
        }
        public async Task<List<ChiTietDh>> GetChiTietDonHangByMaDH(string maDH)
        {
            return await _context.ChiTietDhs
                .AsNoTracking()
                .Include(sp => sp.MaSpNavigation)
                .Where(ct => ct.MaDonHang == maDH)
                .ToListAsync();
        }
        public async Task<bool> UpdateDonHang(DonHang donHang)
        {
            try
            {
                _context.DonHangs.Update(donHang);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteDonHang(string maDH)
        {
            try
            {
                var donHang = await _context.DonHangs.FirstOrDefaultAsync(dh => dh.MaDonHang == maDH);
                if(donHang == null)
                {
                    return false;
                }

                if (donHang.ChiTietDhs != null && donHang.ChiTietDhs.Any())
                {
                    _context.ChiTietDhs.RemoveRange(donHang.ChiTietDhs);
                }

                _context.DonHangs.Remove(donHang);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> CheckAcc(string matk)
        {
            return await _context.TaiKhoans.AnyAsync(tk => tk.MaTaiKhoan == matk);
        }
        public async Task<bool> CheckPTTT(string mapttt)
        {
            return await _context.PhuongThucTts.AnyAsync(pttt => pttt.MaPttt == mapttt);
        }
        public async Task<bool> CheckPTVC(string maptvc)
        {
            return await _context.PhuongThucVcs.AnyAsync(ptvc => ptvc.MaPtvc == maptvc);
        }
        public async Task<bool> CheckVoucher(string mavoucher)
        {
            return await _context.Vouchers.AnyAsync(v => v.MaVoucher == mavoucher
                                                                     && v.NgayBatDau <= DateTime.Now
                                                                     && v.NgayKetThuc >= DateTime.Now);
        }
        public async Task<Voucher?> GetVoucherByMaVoucher(string mavoucher)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(v => v.MaVoucher == mavoucher && v.NgayBatDau <= DateTime.Now && v.NgayKetThuc >= DateTime.Now);
        }
    }
}
