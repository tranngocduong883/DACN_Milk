using DACN_H_P.Dtos.Request;
using DACN_H_P.Dtos.Response;

namespace DACN_H_P.Service
{
    public interface IDanhGiaService
    {
        // User
        Task<(bool Success, string Message, DanhGiaResponse? Data)> TaoDanhGia(TaoDanhGiaRequest request);
        Task<(bool Success, string Message, DanhGiaResponse? Data)> SuaDanhGia(SuaDanhGiaRequest request);

        // Admin
        Task<(bool Success, string Message, DanhGiaResponse? Data)> DuyetDanhGia(string maDanhGia, string trangThaiMoi);
        Task<(bool Success, string Message)> XoaDanhGia(string maDanhGia);
        Task<IEnumerable<DanhGiaResponse>> AdminGetAll();

        // Public (Ai cũng xem được)
        Task<IEnumerable<DanhGiaResponse>> GetByProduct(string maSp);
    }
}
