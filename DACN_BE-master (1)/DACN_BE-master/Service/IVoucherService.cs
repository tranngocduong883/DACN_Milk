using DACN_H_P.Dtos.Request;
using DACN_H_P.Dtos.Response;

namespace DACN_H_P.Service
{
    public interface IVoucherService
    {
        Task<IEnumerable<VoucherResponse>> GetAllVouchers();
        Task<VoucherResponse?> GetVoucherByCode(string maVoucher);
        Task<(bool Success, string Message, VoucherResponse? response)> CreateVoucher(VoucherRequest request);
        Task<(bool Success, string Message, VoucherResponse? response)> UpdateVoucher(VoucherRequest request);
        Task<(bool Success, string Message)> DeleteVoucher(string maVoucher);
        Task<IEnumerable<VoucherResponse>> GetVouchersForUser(string maTk);
    }
}
