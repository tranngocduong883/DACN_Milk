using DACN_H_P.Dtos.Request;
using DACN_H_P.Dtos.Response;
using DACN_H_P.Model;

namespace DACN_H_P.Service
{
    public interface ITaiKhoanService
    {
        Task<(bool success, string message)> DangKyAsync(DangKyRequest request);
        Task<(bool success, string message, DangNhapResponse response)> DangNhapAsync(DangNhapRequest request);
        Task<IEnumerable<TaiKhoanResponse>> GetAllTaiKhoan();
        Task<TaiKhoanResponse?> GetTaiKhoanById(string id);

        Task<(bool Success, string Message, TaiKhoanResponse? response)> UpdateProfile(string id, UpdateProfileRequest request);
        Task<(bool Success, string Message, TaiKhoanResponse? response)> ChangePassword(string id, ChangePasswordRequest request);

        Task<(bool Success, string Message, TaiKhoanResponse? response)> UpdateRole(string id, string newRole);
        Task<(bool Success, string Message)> DeleteTaiKhoan(string id);
    }
}
