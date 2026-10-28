using DACN_H_P.Dtos.Response;

namespace DACN_H_P.Service
{
    public interface ICuaHangService
    {
        Task<IEnumerable<CuaHangResponse>> GetDSCH();
    }
}
