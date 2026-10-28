using DACN_H_P.Model;
using Microsoft.EntityFrameworkCore;

namespace DACN_H_P.Repository.Impl
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly DacnHContext _context;

        public VoucherRepository(DacnHContext context)
        {
            _context = context;
        }
        public async Task<List<Voucher>> GetAllVouchers()
        {
            return await _context.Vouchers.AsNoTracking().OrderByDescending(v => v.NgayBatDau).ToListAsync();
        }

        public async Task<Voucher?> GetVoucherById(string maVoucher)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(v => v.MaVoucher == maVoucher);
        }

        public async Task<bool> CreateVoucher(Voucher voucher)
        {
            try
            {
                await _context.Vouchers.AddAsync(voucher);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> UpdateVoucher(Voucher voucher)
        {
            try
            {
                _context.Vouchers.Update(voucher);
                await _context.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteVoucher(string maVoucher)
        {
            try
            {
                var exist = await _context.Vouchers.FindAsync(maVoucher);
                if (exist == null) return false;

                _context.Vouchers.Remove(exist);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<List<Voucher>> GetVouchersActive()
        {
            var now = DateTime.Now;
            return await _context.Vouchers
                .AsNoTracking()
                .Where(v => v.NgayBatDau <= now && v.NgayKetThuc >= now)
                .OrderBy(v => v.DieuKienApDung)
                .ToListAsync();
        }

        public async Task<List<string>> GetUsedVoucherCodes(string maTk)
        {
            return await _context.DonHangs
                .AsNoTracking()
                .Where(dh => dh.MaTaiKhoan == maTk && dh.MaVoucher != null)
                .Select(dh => dh.MaVoucher!)
                .Distinct()
                .ToListAsync();
        }
    }
}
