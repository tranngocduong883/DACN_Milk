using DACN_H_P.Dtos.Request;
using DACN_H_P.Dtos.Response;
using DACN_H_P.Mapper;
using DACN_H_P.Model;
using DACN_H_P.Repository;
using DACN_H_P.Utils;

namespace DACN_H_P.Service.Impl
{
    public class DanhGiaService : IDanhGiaService
    {
        private readonly IDanhGiaRepository _repo;

        public DanhGiaService(IDanhGiaRepository repo)
        {
            _repo = repo;
        }

        public async Task<(bool, string, DanhGiaResponse?)> TaoDanhGia(TaoDanhGiaRequest request)
        {
            // 1. Check xem đã mua hàng chưa (Logic chống phá đám)
            var daMua = await _repo.CheckUserBoughtProduct(request.MaTaiKhoan, request.MaSp);
            if (!daMua)
            {
                return (false, "Bạn phải mua và nhận hàng thành công sản phẩm này mới được đánh giá", null);
            }

            var daReview = await _repo.CheckUserReviewed(request.MaTaiKhoan, request.MaSp);
            if (daReview) return (false, "Bạn đã đánh giá sản phẩm này rồi, hãy dùng chức năng sửa", null);

            var danhGia = new DanhGiaSp
            {
                MaDanhGia = Guid.NewGuid().ToString(),
                MaSp = request.MaSp,
                MaTaiKhoan = request.MaTaiKhoan,
                SoSao = request.SoSao,
                BinhLuan = request.BinhLuan,
                NgayCapNhat = DateTime.Now,
                TrangThaiDg = TrangThaiDanhGia.ChoDuyet // Mặc định chờ Admin duyệt
            };

            await _repo.CreateAsync(danhGia);

            // Lấy lại info đầy đủ để map tên SP/User
            var result = await _repo.GetByMaDanhGia(danhGia.MaDanhGia);
            return (true, "Gửi đánh giá thành công, vui lòng chờ duyệt", DanhGiaBuilder.ToResponse(result!));
        }

        public async Task<(bool, string, DanhGiaResponse?)> SuaDanhGia(SuaDanhGiaRequest request)
        {
            var dg = await _repo.GetByMaDanhGia(request.MaDanhGia);
            if (dg == null) return (false, "Đánh giá không tồn tại", null);

            // Check chính chủ
            if (dg.MaTaiKhoan != request.MaTaiKhoan) return (false, "Không có quyền sửa đánh giá này", null);

            dg.SoSao = request.SoSao;
            dg.BinhLuan = request.BinhLuan;
            dg.NgayCapNhat = DateTime.Now;

            // QUAN TRỌNG: Sửa xong thì quay về Chờ Duyệt để tránh user sửa nội dung bậy bạ
            dg.TrangThaiDg = TrangThaiDanhGia.ChoDuyet;

            await _repo.UpdateAsync(dg);
            return (true, "Cập nhật thành công, đang chờ duyệt lại", DanhGiaBuilder.ToResponse(dg));
        }

        public async Task<(bool, string, DanhGiaResponse?)> DuyetDanhGia(string maDanhGia, string trangThaiMoi)
        {
            var dg = await _repo.GetByMaDanhGia(maDanhGia);
            if (dg == null) return (false, "Đánh giá không tồn tại", null);

            // Validate trạng thái
            if (trangThaiMoi != TrangThaiDanhGia.DaDuyet && trangThaiMoi != TrangThaiDanhGia.KhongHopLe)
            {
                return (false, "Trạng thái duyệt không hợp lệ", null);
            }

            dg.TrangThaiDg = trangThaiMoi;
            await _repo.UpdateAsync(dg);

            return (true, $"Đã cập nhật trạng thái thành {trangThaiMoi}", DanhGiaBuilder.ToResponse(dg));
        }

        public async Task<(bool, string)> XoaDanhGia(string maDanhGia)
        {
            var dg = await _repo.GetByMaDanhGia(maDanhGia);
            if (dg == null) return (false, "Đánh giá không tồn tại");

            await _repo.DeleteAsync(dg);
            return (true, "Đã xóa đánh giá");
        }

        public async Task<IEnumerable<DanhGiaResponse>> AdminGetAll()
        {
            var list = await _repo.GetAllForAdmin();
            return list.Select(DanhGiaBuilder.ToResponse);
        }

        public async Task<IEnumerable<DanhGiaResponse>> GetByProduct(string maSp)
        {
            var list = await _repo.GetByMaSp(maSp);
            return list.Select(DanhGiaBuilder.ToResponse);
        }
    }
}
