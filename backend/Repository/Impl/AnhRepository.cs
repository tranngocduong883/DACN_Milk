using DACN_H_P.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DACN_H_P.Repository.Impl
{
    public class AnhRepository : IAnhRepository
    {
        private readonly DacnHContext _context;
        public AnhRepository(DacnHContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AnhSp>> GetAllAnh()
        {
            return await _context.AnhSps.ToListAsync();
        }

        public async Task<IEnumerable<AnhSp>> GetByMaSP(string maSP)
        {
            var exist = _context.AnhSps.Where(a => a.MaSp == maSP);
            if(!await exist.AnyAsync())
            {
                return null;
            }
            var data = await exist.ToListAsync();
            return data;
        }
        public async Task<IEnumerable<AnhSp>> PostAsync(List<AnhSp> imgs)
        {
            if (imgs == null || imgs.Count == 0)
            {
                return new List<AnhSp>();
            }
            await _context.AnhSps.AddRangeAsync(imgs);
            await _context.SaveChangesAsync();
            var masp = imgs.First().MaSp;
            return await _context.AnhSps.Where(a => a.MaSp == masp).ToListAsync();
        }

        public async Task<bool> DeleteAsync(string maanh)
        {
            var exist = _context.AnhSps.Where(a => a.MaAnh == maanh);
            if(!await exist.AnyAsync())
            {
                return false;
            }
            _context.AnhSps.RemoveRange(exist);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CheckSP(string maSP)
        {
            var exist = await _context.SanPhams.FirstOrDefaultAsync(sp => sp.MaSp == maSP);
            if(exist == null)
            {
                return false;
            }
            return true;
        }
    }
}
