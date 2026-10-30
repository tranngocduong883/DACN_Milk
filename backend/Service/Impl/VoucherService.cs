using DACN_H_P.Dtos.Request;
using DACN_H_P.Dtos.Response;
using DACN_H_P.Mapper;
using DACN_H_P.Model;
using DACN_H_P.Repository;

namespace DACN_H_P.Service.Impl
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _repo;

        public VoucherService(IVoucherRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<VoucherResponse>> GetAllVouchers()
        {
            var data = await _repo.GetAllVouchers();
            return data.Select(VoucherBuilder.ToResponse);
        }
        public async Task<VoucherResponse?> GetVoucherByCode(string maVoucher)
        {
            var data = await _repo.GetVoucherById(maVoucher);
            return data == null ? null : VoucherBuilder.ToResponse(data);
        }

        public async Task<(bool, string, VoucherResponse?)> CreateVoucher(VoucherRequest request)
        {
            // Validate Logic
            if (request.NgayKetThuc <= request.NgayBatDau)
            {
                return (false, "Ngày kết thúc phải sau ngày bắt đầu", null);
            }

            if(request.DieuKienApDung < 0)
            {
                return (false, "Điều kiện áp dụng phải lớn hơn 0", null);
            }

            var exist = await _repo.GetVoucherById(request.MaVoucher);
            if (exist != null)
            {
                return (false, $"Mã voucher {request.MaVoucher} đã tồn tại", null);
            }

            Voucher newVoucher = new Voucher();
            newVoucher.MaVoucher = request.MaVoucher;
            newVoucher.TenVoucher = request.TenVoucher;
            newVoucher.GiamGia = request.GiamGia;
            newVoucher.NgayBatDau = request.NgayBatDau;
            newVoucher.NgayKetThuc = request.NgayKetThuc;
            newVoucher.DieuKienApDung = request.DieuKienApDung;
            newVoucher.MoTa = request.MoTa;
            var success = await _repo.CreateVoucher(newVoucher);

            if (!success) return (false, "Lỗi hệ thống khi thêm", null);

            return (true, "Thêm thành công", VoucherBuilder.ToResponse(newVoucher));
        }

        public async Task<(bool, string, VoucherResponse?)> UpdateVoucher(VoucherRequest request)
        {
            if (request.NgayKetThuc <= request.NgayBatDau)
            {
                return (false, "Ngày kết thúc phải sau ngày bắt đầu", null);
            }

            if (request.DieuKienApDung < 0)
            {
                return (false, "Điều kiện áp dụng phải lớn hơn 0", null);
            }

            var exist = await _repo.GetVoucherById(request.MaVoucher);
            if (exist == null)
            {
                return (false, "Voucher không tồn tại", null);
            }

            exist.TenVoucher = request.TenVoucher;
            exist.GiamGia = request.GiamGia;
            exist.DieuKienApDung = request.DieuKienApDung;
            exist.NgayBatDau = request.NgayBatDau;
            exist.NgayKetThuc = request.NgayKetThuc;
            exist.MoTa = request.MoTa;

            var success = await _repo.UpdateVoucher(exist);
            if (!success)
            {
                return (false, "Lỗi khi cập nhật", null);
            }
            return (true, "Cập nhật thành công", VoucherBuilder.ToResponse(exist));
        }
        public async Task<(bool, string)> DeleteVoucher(string maVoucher)
        {
            var success = await _repo.DeleteVoucher(maVoucher);
            if (!success)
            {
                return (false, "Không thể xóa (Voucher không tồn tại hoặc đã được sử dụng trong đơn hàng)");
            }
            return (true, "Xóa thành công");
        }
        public async Task<IEnumerable<VoucherResponse>> GetVouchersForUser(string maTk)
        {
            var allActiveVouchers = await _repo.GetVouchersActive();

            var usedVoucher = await _repo.GetUsedVoucherCodes(maTk);

            var availableVouchers = allActiveVouchers
                .Where(v => !usedVoucher.Contains(v.MaVoucher))
                .ToList();

            return availableVouchers.Select(VoucherBuilder.ToResponse).ToList();
        }
    }
}
