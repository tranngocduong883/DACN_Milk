using DACN_H_P.Model;

namespace DACN_H_P.Repository
{
    public interface IVoucherRepository
    {
        Task<List<Voucher>> GetAllVouchers();
        Task<Voucher?> GetVoucherById(string maVoucher);
        Task<bool> CreateVoucher(Voucher voucher);
        Task<bool> UpdateVoucher(Voucher voucher);
        Task<bool> DeleteVoucher(string maVoucher);
        Task<List<Voucher>> GetVouchersActive();
        Task<List<string>> GetUsedVoucherCodes(string maTk);
    }
}
