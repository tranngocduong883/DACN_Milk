using DACN_H_P.Dtos.Response;
using DACN_H_P.Model;

namespace DACN_H_P.Repository
{
    public interface IAnhRepository
    {
        Task<IEnumerable<AnhSp>> GetAllAnh();
        Task<IEnumerable<AnhSp>> GetByMaSP(string maSP);
        Task<IEnumerable<AnhSp>> PostAsync(List<AnhSp> anhSps);
        Task<bool> DeleteAsync(string maanh);
        Task<bool> CheckSP(string maSP);
    }
}
