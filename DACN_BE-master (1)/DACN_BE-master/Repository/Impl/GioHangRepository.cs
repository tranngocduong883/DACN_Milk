using DACN_H_P.Dtos.Response;
using DACN_H_P.Model;
using Microsoft.EntityFrameworkCore;

namespace DACN_H_P.Repository.Impl
{
    public class GioHangRepository : IGioHangRepository
    {
        private readonly DacnHContext _context;

        public GioHangRepository(DacnHContext context)
        {
            _context = context;
        }
        public async Task<GioHang?> GetGioHangByMaTaiKhoanAsync(string matk)
        {
            return await _context.GioHangs
                .Include(GH => GH.ChiTietGhs)
                .ThenInclude(GH => GH.MaSpNavigation)
                .ThenInclude(sp => sp.AnhSps)
                .Include(GH => GH.ChiTietGhs)
                .ThenInclude(sp => sp.MaSpNavigation)
                .ThenInclude(ctkm => ctkm.ChiTietKms)
                .ThenInclude(km => km.MaKhuyenMaiNavigation)
                .FirstOrDefaultAsync(GH => GH.MaTaiKhoan == matk);
        }

        public async Task<bool> AddOrUpdateGioHang(string matk, string masp, int soluong)
        {
            var tk = await _context.TaiKhoans.FirstOrDefaultAsync(tk => tk.MaTaiKhoan == matk);
            if (tk == null)
            {
                return false;
            }
            var gioHang = await _context.GioHangs.Include(GH => GH.ChiTietGhs)
                .FirstOrDefaultAsync(GH => GH.MaTaiKhoan == matk);
            if (gioHang == null)
            {
                gioHang = new GioHang
                {
                    MaGioHang = Guid.NewGuid().ToString(),
                    MaTaiKhoan = matk,
                    ChiTietGhs = new List<ChiTietGh>()
                };
                _context.GioHangs.Add(gioHang);
            }
            var item = gioHang.ChiTietGhs.FirstOrDefault(i => i.MaSp == masp);
            if (item == null)
            {
                item = new ChiTietGh
                {
                    MaGioHang = gioHang.MaGioHang,
                    MaSp = masp,
                    SoLuong = soluong
                };
                gioHang.ChiTietGhs.Add(item);
            }
            else
            {
                item.SoLuong += soluong;
            }
            return true;
        }
        public async Task<bool> DecreaseItemGioHang(string matk, string masp, int soluong)
        {
            var tk = await _context.TaiKhoans.FirstOrDefaultAsync(tk => tk.MaTaiKhoan == matk);
            if (tk == null)
            {
                return false;
            }
            var gioHang = await _context.GioHangs
                .Include(GH => GH.ChiTietGhs)
                .FirstOrDefaultAsync(GH => GH.MaTaiKhoan == matk);
            if (gioHang == null)
            {
                return false;
            }
            var item = gioHang.ChiTietGhs.FirstOrDefault(i => i.MaSp == masp);
            if (item != null)
            {
                item.SoLuong -= soluong;
            }
            else
            {
                return false;
            }
            return true;
        }
        public async Task<bool> UpdateItemGioHang(string matk, string masp, int soluong)
        {
            var tk = await _context.TaiKhoans.FirstOrDefaultAsync(tk => tk.MaTaiKhoan == matk);
            if (tk == null)
            {
                return false;
            }
            var gioHang = await _context.GioHangs
                .Include(GH => GH.ChiTietGhs)
                .FirstOrDefaultAsync(GH => GH.MaTaiKhoan == matk);
            if (gioHang == null)
            {
                return false;
            }
            var item = gioHang.ChiTietGhs.FirstOrDefault(i => i.MaSp == masp);
            if (item != null)
            {
                item.SoLuong = soluong;
            }
            else
            {
                return false;
            }
            return true;
        }
        public async Task<bool> RemoveItemAsync(string matk, string masp)
        {
            var tk = await _context.TaiKhoans.FirstOrDefaultAsync(tk => tk.MaTaiKhoan == matk);
            if (tk == null)
            {
                return false;
            }
            var gioHang = await _context.GioHangs
                .Include(GH => GH.ChiTietGhs)
                .FirstOrDefaultAsync(GH => GH.MaTaiKhoan == matk);
            if (gioHang == null)
            {
                return false;
            }
            var item = gioHang.ChiTietGhs.FirstOrDefault(i => i.MaSp == masp);
            if (item != null)
            {
                gioHang.ChiTietGhs.Remove(item);
            }
            return true;
        }

        public async Task<bool> RemoveAllGioHang(string matk)
        {
            var tk = await _context.TaiKhoans.FirstOrDefaultAsync(tk => tk.MaTaiKhoan == matk);
            if (tk == null)
            {
                return false;
            }
            var gioHang = await _context.GioHangs
                .Include(GH => GH.ChiTietGhs)
                .FirstOrDefaultAsync(GH => GH.MaTaiKhoan == matk);
            if (gioHang != null)
            {
                _context.ChiTietGhs.RemoveRange(gioHang.ChiTietGhs);
                await _context.SaveChangesAsync();
            }
            return true;
        }
        public async Task SavechangeAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
