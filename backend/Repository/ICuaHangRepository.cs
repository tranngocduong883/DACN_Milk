using DACN_H_P.Model;

namespace DACN_H_P.Repository
{
    public interface ICuaHangRepository
    {
        Task<List<CuaHang>> GetDSCH();
    }
}
