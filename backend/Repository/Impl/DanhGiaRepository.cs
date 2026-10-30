using DACN_H_P.Model;
using DACN_H_P.Utils;
using Microsoft.EntityFrameworkCore;

namespace DACN_H_P.Repository.Impl
{
    public class DanhGiaRepository : IDanhGiaRepository
    {
        private readonly DacnHContext _context;

        public DanhGiaRepository(DacnHContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(DanhGiaSp dg)
        {
            await _context.DanhGiaSps.AddAsync(dg);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DanhGiaSp dg)
        {
            _context.DanhGiaSps.Update(dg);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(DanhGiaSp dg)
        {
            _context.DanhGiaSps.Remove(dg);
            await _context.SaveChangesAsync();
        }

        public async Task<DanhGiaSp?> GetByMaDanhGia(string maDanhGia)
        {
            return await _context.DanhGiaSps
                .Include(d => d.MaSpNavigation)
                .Include(d => d.MaTaiKhoanNavigation)
                .FirstOrDefaultAsync(d => d.MaDanhGia == maDanhGia);
        }

        public async Task<List<DanhGiaSp>> GetAllForAdmin()
        {
            return await _context.DanhGiaSps
                .AsNoTracking()
                .Include(d => d.MaSpNavigation)
                .Include(d => d.MaTaiKhoanNavigation)
                .OrderByDescending(d => d.NgayCapNhat)
                .ToListAsync();
        }

        public async Task<List<DanhGiaSp>> GetByMaSp(string maSp)
        {
            // Chỉ lấy những đánh giá "DaDuyet" cho người dùng xem
            return await _context.DanhGiaSps
                .AsNoTracking()
                .Include(d => d.MaTaiKhoanNavigation)
                .Where(d => d.MaSp == maSp && d.TrangThaiDg == TrangThaiDanhGia.DaDuyet)
                .OrderByDescending(d => d.NgayCapNhat)
                .ToListAsync();
        }

        // --- LOGIC QUAN TRỌNG NHẤT ---
        public async Task<bool> CheckUserBoughtProduct(string maTk, string maSp)
        {
            // Logic: Tìm trong bảng Đơn hàng -> Chi tiết đơn hàng
            // Điều kiện: MaTK khớp + MaSp khớp + Trạng thái đơn phải là GiaoHangThanhCong
            return await _context.DonHangs
                .AnyAsync(dh => dh.MaTaiKhoan == maTk
                             && dh.TrangThaiDonHang == TrangThaiDonHang.GiaoHangThanhCong // Phải giao xong mới đc đánh giá
                             && dh.ChiTietDhs.Any(ct => ct.MaSp == maSp));
        }

        public async Task<bool> CheckUserReviewed(string maTk, string maSp)
        {
            return await _context.DanhGiaSps.AnyAsync(dg => dg.MaTaiKhoan == maTk && dg.MaSp == maSp);
        }
    }
}
