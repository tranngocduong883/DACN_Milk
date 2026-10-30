using DACN_H_P.Dtos.Request;
using DACN_H_P.Dtos.Response;

namespace DACN_H_P.Service
{
    public interface IAnhService
    {
        Task<IEnumerable<AnhResponse>> GetAllAnh();
        Task<IEnumerable<AnhResponse>> GetByMaSP(string maSP);
        Task<(bool success, string message, IEnumerable<AnhResponse> response)> PostAsync(AnhRequest request);
        Task<(bool success, string message)> DeleteAsync(string maanh);
    }
}
