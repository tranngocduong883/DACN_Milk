using DACN_H_P.Dtos.Response;
using DACN_H_P.Repository;
using DACN_H_P.Mapper;

namespace DACN_H_P.Service.Impl
{
    public class CuaHangService : ICuaHangService
    {
        private readonly ICuaHangRepository _repo;

        public CuaHangService(ICuaHangRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<CuaHangResponse>> GetDSCH()
        {
            var result = await _repo.GetDSCH();
            return result.Select(CuaHangBuilder.ToResponse);
        }
    }
}
