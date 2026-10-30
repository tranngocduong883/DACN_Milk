using DACN_H_P.Dtos.Request;
using DACN_H_P.Dtos.Response;
using DACN_H_P.Model;

namespace DACN_H_P.Service
{
    public interface IDanhMucService
    {
        Task<IEnumerable<DanhMucResponse>> GetAllAsync();
        Task<DanhMucResponse> GetByMaDMAsync(string madm);
        Task<IEnumerable<DanhMucResponse>> GetByTenDMAsync(string tendm);
        Task<(bool success, string message, DanhMucResponse response)> PostAsync(DanhMucRequest request);
        Task<(bool success, string message, DanhMucResponse response)> EditAsync(DanhMucRequest request);
        Task<(bool succes, string message)> DeleteAsync(string madm);
    }
}
