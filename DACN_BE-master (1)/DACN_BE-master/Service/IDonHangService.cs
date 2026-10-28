using DACN_H_P.Dtos.Request;
using DACN_H_P.Dtos.Response;

namespace DACN_H_P.Service
{
    public interface IDonHangService
    {
        Task<(bool success, string message, DonHangResponse? response)> CreateDonHang(DatHangRequest request);
        Task<DonHangResponse?> GetDonHangByMaDH(string maDH);
        Task<IEnumerable<DonHangResponse>?> GetDonHangByMaTK(string maTK);
        Task<IEnumerable<DonHangResponse>> GetAllDonHang();
        Task<(bool Success, string Message, DonHangResponse? response)> UpdateTrangThaiDonHang(string maDonHang, string trangThaiMoi);
        Task<(bool Success, string Message, DonHangResponse? response)> HuyDonHang(string maDonHang, string maTaiKhoan);
        Task<(bool Success, string Message, DonHangResponse? response)> UpdateDiaChiGiaoHang(string maDonHang, string maTaiKhoan, string diaChiMoi);
        Task<(bool Success, string Message)> DeleteDonHang(string maDH);
    }
}
