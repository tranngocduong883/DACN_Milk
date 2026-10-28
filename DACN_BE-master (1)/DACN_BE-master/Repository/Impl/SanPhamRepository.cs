using DACN_H_P.Model;
using Microsoft.EntityFrameworkCore;

namespace DACN_H_P.Repository.Impl
{
    public class SanPhamRepository: ISanPhamRepository
    {
        private readonly DacnHContext _context;
        public SanPhamRepository(DacnHContext context)
        {
            _context = context;
        }
        public async Task<bool> DeleteAsync(string masp)
        {
            var exist = await _context.SanPhams.FirstOrDefaultAsync(p => p.MaSp == masp);
            if(exist == null || await _context.ChiTietDhs.AnyAsync(ct => ct.MaSp == masp))
            {
                return false;
            }
            _context.AnhSps.RemoveRange(exist.AnhSps);
            _context.SanPhams.Remove(exist);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<SanPham>> GetAllAsync()
        {
            return await _context.SanPhams.Include(p => p.MaDanhMucNavigation).Include(p => p.AnhSps).Include(p => p.ChiTietKms).ThenInclude(p => p.MaKhuyenMaiNavigation).ToListAsync();
        }
        public async Task<SanPham?> GetByMaSP(string masp)
        {
            var exist = await _context.SanPhams.Include(p => p.MaDanhMucNavigation).Include(p => p.AnhSps).Include(p => p.ChiTietKms).ThenInclude(p => p.MaKhuyenMaiNavigation)
                .FirstOrDefaultAsync(p => p.MaSp == masp);
            return exist;
        }
        public async Task<IEnumerable<SanPham?>> GetByTenSP(string tensp)
        {
            var exist = _context.SanPhams.Include(p => p.MaDanhMucNavigation).Include(p => p.AnhSps).Include(p => p.ChiTietKms).ThenInclude(p => p.MaKhuyenMaiNavigation)
                .Where(p => p.TenSp.ToLower().Contains(tensp.ToLower()));
            if(!await exist.AnyAsync())
            {
                return null;
            }
            var data = await exist.ToListAsync();
            return data;
        }
        public async Task<IEnumerable<SanPham?>> GetByMaDM(string madm)
        {
            var danhmuc = await _context.DanhMucs.FirstOrDefaultAsync(dm => dm.MaDanhMuc == madm);
            if(danhmuc == null)
            {
                return null;
            }
            var exist = await _context.SanPhams.Include(p => p.MaDanhMucNavigation).Include(p => p.AnhSps).Include(p => p.ChiTietKms).ThenInclude(p => p.MaKhuyenMaiNavigation)
                .Where(p => p.MaDanhMuc == madm).ToListAsync();
            return exist;
        }
        public async Task<SanPham?> PostAsync(SanPham sanPham)
        {
            await _context.SanPhams.AddAsync(sanPham);
            await _context.SaveChangesAsync();
            return sanPham;
        }
        public async Task<SanPham?> PutAsync(SanPham sanPham)
        {
            var exist = await _context.SanPhams.Include(p => p.MaDanhMucNavigation).Include(p => p.AnhSps)
                .FirstOrDefaultAsync(p => p.MaSp == sanPham.MaSp);
            if (exist == null)
            {
                return null;
            }
            _context.Entry(exist).CurrentValues.SetValues(sanPham);
            await _context.SaveChangesAsync();
            return exist;
        }

        public async Task<SanPham?> GetFullInfoByMaSP(string masp)
        {
            var exist = await _context.SanPhams.Include(p => p.MaDanhMucNavigation).Include(p => p.AnhSps).Include(p => p.ChiTietKms).ThenInclude(p => p.MaKhuyenMaiNavigation)
                .FirstOrDefaultAsync(p => p.MaSp == masp);
            return exist;
        }
    }
}
