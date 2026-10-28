using DACN_H_P.Model;
using Microsoft.EntityFrameworkCore;

namespace DACN_H_P.Repository.Impl
{
    public class DanhMucRepository:IDanhMucRepository
    {
        private readonly DacnHContext _context;
        public DanhMucRepository(DacnHContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DanhMuc>> GetAllAsync()
        {
            return await _context.DanhMucs.ToListAsync();
        }

        public async Task<DanhMuc> GetByMaDMAsync(string maDm)
        {
            return await _context.DanhMucs.FirstOrDefaultAsync(dm => dm.MaDanhMuc == maDm);
        }

        public async Task<IEnumerable<DanhMuc>> GetByTenDMAsync(string tenDm)
        {
            var result = _context.DanhMucs.Where(dm => dm.TenDanhMuc.ToLower().Contains(tenDm));
            if(!await result.AnyAsync())
            {
                return null;
            }
            var data = await result.ToListAsync();
            return data;
        }


        public async Task<DanhMuc?> PostAsync(DanhMuc dm)
        {
            await _context.DanhMucs.AddAsync(dm);
            await _context.SaveChangesAsync();
            return dm;
        }

        public async Task<DanhMuc?> EditAsync(DanhMuc dm)
        {
            var exist = await _context.DanhMucs.FirstOrDefaultAsync(Dm => Dm.MaDanhMuc == dm.MaDanhMuc);
            if(exist  == null)
            {
                return null;
            }
            else
            {
                _context.Entry(exist).CurrentValues.SetValues(dm);
                await _context.SaveChangesAsync();
                return exist;
            }
        }

        public async Task<bool> DeleteAsync(string madm)
        {
            var exist = await _context.DanhMucs.FirstOrDefaultAsync(Dm => Dm.MaDanhMuc == madm);
            // Không xóa danh mục còn sản phẩm (khóa ngoại SAN_PHAM.maDanhMuc)
            if(exist == null || await _context.SanPhams.AnyAsync(sp => sp.MaDanhMuc == madm))
            {
                return false;
            }
            else
            {
                _context.DanhMucs.Remove(exist);
                await _context.SaveChangesAsync();
                return true;
            }
        }
    }
}
