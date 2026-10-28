using DACN_H_P.Dtos.Request;
using DACN_H_P.Dtos.Response;
using DACN_H_P.Mapper;
using DACN_H_P.Model;
using DACN_H_P.Repository;
using System.Net.WebSockets;

namespace DACN_H_P.Service.Impl
{
    public class KhuyenMaiService : IKhuyenMaiService
    {
        private readonly IKhuyenMaiRepository _repo;
        public KhuyenMaiService(IKhuyenMaiRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<KhuyenMaiResponse>> GetAllKM()
        {
            var result = await _repo.GetAllKM();
            return result.Select(KhuyenMaiBuilder.ToResponse).ToList();
        }
        public async Task<KhuyenMaiResponse?> GetKMByMaKM(string makm)
        {
            var result = await _repo.GetKMByMaKM(makm);
            if(result == null)
            {
                return null;
            }
            return KhuyenMaiBuilder.ToResponse(result);
        }
        public async Task<(bool, string, KhuyenMaiResponse)> PostKM(KhuyenMaiRequest request)
        {
            var exist = await _repo.GetKMByMaKM(request.MaKhuyenMai);
            if(exist != null)
            {
                return (false, $"Đã có mã khuyến mãi này {request.MaKhuyenMai}", null);
            }
            if(request.NgayBatDau > request.NgayKetThuc)
            {
                return (false, "Nhập sai về ngày khuyến mãi", null);
            }
            var newKM = new KhuyenMai();
            newKM.MaKhuyenMai = request.MaKhuyenMai;
            newKM.TenKhuyenMai = request.TenKhuyenMai;
            newKM.NgayKetThuc = request.NgayKetThuc;
            newKM.NgayBatDau = request.NgayBatDau;
            newKM.MoTa = request.MoTa;
            var data = await _repo.PostKM(newKM);
            if (!data)
            {
                return (false, "Có lỗi khi thêm", null);
            }
            return (true, "Thêm thành công khuyến mãi", KhuyenMaiBuilder.ToResponse(newKM));
        }
        public async Task<(bool, string, KhuyenMaiResponse)> UpdateKM(KhuyenMaiRequest request)
        {
            var exist = await _repo.GetKMByMaKM(request.MaKhuyenMai);
            if(exist == null)
            {
                return (false, "Khuyến mãi không tồn tại", null);
            }
            if (request.NgayBatDau > request.NgayKetThuc)
            {
                return (false, "Nhập sai về ngày khuyến mãi", null);
            }
            exist.TenKhuyenMai = request.TenKhuyenMai;
            exist.NgayBatDau = request.NgayBatDau;
            exist.NgayKetThuc = request.NgayKetThuc;
            exist.MoTa = request.MoTa;
            var success = await _repo.UpdateKM(exist);
            if (!success)
            {
                return (false, "Có lỗi khi sửa", null);
            }
            return (true, "Sửa thành công", KhuyenMaiBuilder.ToResponse(exist));
        }
        public async Task<(bool, string)> DeleteKM(string makm)
        {
            var exist = await _repo.GetKMByMaKM(makm);
            if(exist == null)
            {
                return (false, "Không có khuyến mãi này");
            }
            var result = await _repo.DeleteKM(makm);
            if (!result)
            {
                return (false, "Có lỗi xảy ra khi xóa");
            }
            return (true, "Xóa thành công");
        }
        public async Task<IEnumerable<ChiTietKhuyenMaiResponse>?> GetChiTietKMByMaKM(string makm)
        {
            var exist = await _repo.CheckKmExist(makm);
            if(!exist)
            {
                return null;
            }
            var result = await _repo.GetChiTietKmByMaKM(makm);
            return result.Select(ChiTietKhuyenMaiBuilder.ToResponse).ToList();
        }
        public async Task<IEnumerable<ChiTietKhuyenMaiResponse>?> GetChiTietKMByMaSP(string masp)
        {
            var exist = await _repo.CheckSpExist(masp);
            if (!exist)
            {
                return null;
            }
            var result = await _repo.GetChiTietKmByMaSP(masp);
            return result.Select(ChiTietKhuyenMaiBuilder.ToResponse).ToList();
        }
        public async Task<(bool, string, ChiTietKhuyenMaiResponse)> PostChiTietKM(ChiTietKhuyenMaiRequest request)
        {
            if(request.PhanTramGiam >= 100 || request.PhanTramGiam <= 0)
            {
                return (false, "Phần trăm giảm không hợp lệ", null);
            }

            var checkKM = await _repo.CheckKmExist(request.MaKhuyenMai);
            if (!checkKM)
            {
                return (false, $"Mã khuyến mãi '{request.MaKhuyenMai}' không tồn tại", null);
            }
            var checkSP = await _repo.CheckSpExist(request.MaSp);
            if (!checkSP)
            {
                return (false, $"Mã sản phẩm '{request.MaSp}' không tồn tại", null);
            }

            var lapSP = await _repo.CheckChiTietExist(request.MaKhuyenMai, request.MaSp);
            if (lapSP)
            {
                return (false, "Sản phẩm này đã có trong chương trình", null);
            }

            ChiTietKm newCTKM = new ChiTietKm();
            newCTKM.MaKhuyenMai = request.MaKhuyenMai;
            newCTKM.MaSp = request.MaSp;
            newCTKM.PhanTramGiam = request.PhanTramGiam;
            var result = await _repo.PostChiTietKm(newCTKM);
            if (!result)
            {
                return (false, "Có lỗi khi thêm", null);
            }
            return (true, "Thêm thành công sản phẩm vào chương trình", ChiTietKhuyenMaiBuilder.ToResponse(newCTKM));
        }
        public async Task<(bool, string, ChiTietKhuyenMaiResponse)> UpdateChiTietKM(ChiTietKhuyenMaiRequest request)
        {
            if (request.PhanTramGiam >= 100 || request.PhanTramGiam <= 0)
            {
                return (false, "Phần trăm giảm không hợp lệ", null);
            }

            var existCTKM = await _repo.GetChiTietKm(request.MaKhuyenMai, request.MaSp);
            if (existCTKM == null)
            {
                return (false, "Sản phẩm này không có trong chương trình", null);
            }
            existCTKM.PhanTramGiam = request.PhanTramGiam;
            var result = await _repo.UpdateChiTietKm(existCTKM);
            if (!result)
            {
                return (false, "Có lỗi khi sửa", null);
            }
            return (true, "Cập nhật thành công", ChiTietKhuyenMaiBuilder.ToResponse(existCTKM));
        }
        public async Task<(bool, string)> DeleteChiTietKM(string makm, string masp)
        {
            var result = await _repo.DeleteChiTietKm(makm, masp);
            if (!result)
            {
                return (false, "Không tìm thấy chương trình khuyến mãi này");
            }
            return (true, "Xóa thành công");
        }
    }
}
