using DACN_H_P.Model;
using Microsoft.EntityFrameworkCore;

namespace DACN_H_P.Repository.Impl
{
    public class KhuyenMaiRepository : IKhuyenMaiRepository
    {
        private readonly DacnHContext _context;
        public KhuyenMaiRepository(DacnHContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<KhuyenMai>> GetAllKM()
        {
            return await _context.KhuyenMais.AsNoTracking().ToListAsync();
        }
        public async Task<KhuyenMai?> GetKMByMaKM(string makm)
        {
            return await _context.KhuyenMais.FirstOrDefaultAsync(km => km.MaKhuyenMai == makm);
        }
        public async Task<bool> PostKM(KhuyenMai km)
        {
            try
            {
                await _context.KhuyenMais.AddAsync(km);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateKM(KhuyenMai km)
        {
            try
            {
                var exist = await _context.KhuyenMais.FirstOrDefaultAsync(kmm => kmm.MaKhuyenMai == km.MaKhuyenMai);
                if (exist == null)
                {
                    return false;
                }
                _context.KhuyenMais.Update(km);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteKM(string makm)
        {
            try
            {
                var exist = await _context.KhuyenMais.FirstOrDefaultAsync(km => km.MaKhuyenMai == makm);
                if (exist == null)
                {
                    return false;
                }
                _context.KhuyenMais.Remove(exist);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public async Task<IEnumerable<ChiTietKm>> GetChiTietKmByMaKM(string maKM)
        {
            return await _context.ChiTietKms.Include(sp =>sp.MaSpNavigation).AsNoTracking().Where(ctkm => ctkm.MaKhuyenMai == maKM).ToListAsync();
        }
        public async Task<IEnumerable<ChiTietKm>> GetChiTietKmByMaSP(string maSP)
        {
            var result = await _context.ChiTietKms.Include(sp => sp.MaSpNavigation).AsNoTracking().Where(sp => sp.MaSp == maSP).ToListAsync();
            return result;
        }
        public async Task<bool> PostChiTietKm(ChiTietKm ctkm)
        {
            try
            {
                await _context.ChiTietKms.AddAsync(ctkm);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateChiTietKm(ChiTietKm ctkm)
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteChiTietKm(string maKM, string maSP)
        {
            try
            {
                var exist = await _context.ChiTietKms.FirstOrDefaultAsync(ctkm => ctkm.MaKhuyenMai == maKM && ctkm.MaSp == maSP);
                if (exist == null)
                {
                    return false;
                }
                _context.ChiTietKms.Remove(exist);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            };
        }
        public async Task<ChiTietKm?> GetChiTietKm(string maKM, string maSP)
        {
            return await _context.ChiTietKms.FirstOrDefaultAsync(x => x.MaKhuyenMai == maKM && x.MaSp == maSP);
        }
        public async Task<bool> CheckKmExist(string maKM)
        {
            return await _context.KhuyenMais.AnyAsync(km => km.MaKhuyenMai == maKM);
        }

        public async Task<bool> CheckSpExist(string maSp)
        {
            return await _context.SanPhams.AnyAsync(sp => sp.MaSp == maSp);
        }

        public async Task<bool> CheckChiTietExist(string maKM, string maSp)
        {
            return await _context.ChiTietKms.AnyAsync(ctkm => ctkm.MaKhuyenMai == maKM && ctkm.MaSp == maSp);
        }
    }
}
