using DACN_H_P.Model;
using Microsoft.EntityFrameworkCore;

namespace DACN_H_P.Repository.Impl
{
    public class CuaHangRepository : ICuaHangRepository
    {
        private readonly DacnHContext _context;
        public CuaHangRepository(DacnHContext context)
        {
            _context = context;
        }
        public async Task<List<CuaHang>> GetDSCH()
        {
            return await _context.CuaHangs.ToListAsync();
        }
    }
}
