using DACN_H_P.Dtos.Request;
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
        public async Task<(IEnumerable<SanPham> items, int total)> SearchAsync(SanPhamQuery query)
        {
            var sanPhams = _context.SanPhams.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.MaDM))
            {
                sanPhams = sanPhams.Where(p => p.MaDanhMuc == query.MaDM);
            }
            if (!string.IsNullOrWhiteSpace(query.TuKhoa))
            {
                var tuKhoa = query.TuKhoa.Trim().ToLower();
                sanPhams = sanPhams.Where(p => p.TenSp.ToLower().Contains(tuKhoa));
            }
            if (query.GiaTu.HasValue)
            {
                sanPhams = sanPhams.Where(p => p.Gia >= query.GiaTu.Value);
            }
            if (query.GiaDen.HasValue)
            {
                sanPhams = sanPhams.Where(p => p.Gia <= query.GiaDen.Value);
            }

            // Đếm tổng trước khi phân trang để client tính số trang
            var total = await sanPhams.CountAsync();

            sanPhams = query.SapXep switch
            {
                "gia_asc" => sanPhams.OrderBy(p => p.Gia).ThenBy(p => p.MaSp),
                "gia_desc" => sanPhams.OrderByDescending(p => p.Gia).ThenBy(p => p.MaSp),
                "ten_asc" => sanPhams.OrderBy(p => p.TenSp).ThenBy(p => p.MaSp),
                "ten_desc" => sanPhams.OrderByDescending(p => p.TenSp).ThenBy(p => p.MaSp),
                _ => sanPhams.OrderBy(p => p.MaSp)
            };

            var items = await sanPhams
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Include(p => p.MaDanhMucNavigation).Include(p => p.AnhSps).Include(p => p.ChiTietKms).ThenInclude(p => p.MaKhuyenMaiNavigation)
                .AsSplitQuery()
                .ToListAsync();

            return (items, total);
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
