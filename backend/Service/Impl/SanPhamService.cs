using DACN_H_P.Dtos.Request;
using DACN_H_P.Dtos.Response;
using DACN_H_P.Helper;
using DACN_H_P.Mapper;
using DACN_H_P.Model;
using DACN_H_P.Repository;
using DACN_H_P.Utils;
using Microsoft.EntityFrameworkCore;

namespace DACN_H_P.Service.Impl
{
    public class SanPhamService : ISanPhamService
    {
        private readonly ISanPhamRepository _repo;

        private readonly CloudinaryService _cloudinary;
        public SanPhamService(ISanPhamRepository repo, CloudinaryService cloudinary)
        {
            _repo = repo;

            _cloudinary = cloudinary;
        }
        public async Task<IEnumerable<SanPhamResponse>> GetAllAsync()
        {
            var result = await _repo.GetAllAsync();
            return result.Select(SanPhamBuilder.ToResponse).ToList();
        }

        public async Task<SanPhamResponse?> GetByMaSP(string masp)
        {
            var result = await _repo.GetByMaSP(masp);
            if (result == null)
            {
                return null;
            }
            return SanPhamBuilder.ToResponse(result);
        }
        public async Task<IEnumerable<SanPhamResponse?>> GetByTenSP(string tensp)
        {
            var result = await _repo.GetByTenSP(tensp);
            if (result == null)
            {
                return null;
            }
            return result.Select(SanPhamBuilder.ToResponse).ToList();
        }
        public async Task<IEnumerable<SanPhamResponse?>> GetByMaDM(string madm)
        {
            var result = await _repo.GetByMaDM(madm);
            if (result == null)
            {
                return null;
            }
            return result.Select(SanPhamBuilder.ToResponse).ToList();
        }
        public async Task<(bool, string, SanPhamResponse)> PostAsync(SanPhamRequest request)
        {
            var exist = await _repo.GetByMaSP(request.MaSp);
            if (exist != null)
            {
                return (false, $"Đã tồn tại sản phẩm có mã {request.MaSp}", null);
            }

            var imgUrl = new List<string>();
            if (request.AnhSps != null && request.AnhSps.Count > 0)
            {
                foreach (var file in request.AnhSps)
                {
                    var url = await _cloudinary.UploadImageAsync(file);
                    if (!string.IsNullOrEmpty(url))
                    {
                        imgUrl.Add(url);
                    }
                }
            }
            var data = new SanPham
            {
                MaSp = request.MaSp,
                TenSp = request.TenSp,
                MaDanhMuc = request.MaDanhMuc,
                Gia = request.Gia,
                MoTa = request.MoTa,
                SoLuongTon = request.SoLuongTon,
                TheTich = request.TheTich,
                DonVi = request.DonVi,
                AnhSps = imgUrl.Select(url => new AnhSp
                {
                    MaAnh = Guid.NewGuid().ToString(),
                    DuongDan = url,
                }).ToList()
            };

            var result = await _repo.PostAsync(data);
            var fullResult = await _repo.GetFullInfoByMaSP(result!.MaSp);
            return (true, "Thêm sản phẩm thành công", SanPhamBuilder.ToResponse(fullResult!));
        }
        public async Task<(bool, string, SanPhamResponse)> PutAsync(SanPhamRequest request)
        {
            var exist = await _repo.GetByMaSP(request.MaSp);
            if (exist == null)
            {
                return (false, $"Không tìm thấy sản phẩm có mã {request.MaSp}", null);
            }

            exist.TenSp = request.TenSp;
            exist.MaDanhMuc = request.MaDanhMuc;
            exist.Gia = request.Gia;
            exist.MoTa = request.MoTa;
            exist.SoLuongTon = request.SoLuongTon;
            exist.TheTich = request.TheTich;
            exist.DonVi = request.DonVi;

            var imgUrl = new List<string>();
            if (request.AnhSps != null && request.AnhSps.Count > 0)
            {
                foreach (var file in request.AnhSps)
                {
                    var url = await _cloudinary.UploadImageAsync(file);
                    if (!string.IsNullOrEmpty(url))
                    {
                        imgUrl.Add(url);
                    }
                }
            }
            foreach (var url in imgUrl)
            {
                exist.AnhSps.Add(new AnhSp
                {
                    MaAnh = Guid.NewGuid().ToString(),
                    MaSp = exist.MaSp,
                    DuongDan = url,
                });
            }
            var result = await _repo.PutAsync(exist);
            var update = await _repo.GetFullInfoByMaSP(result!.MaSp);
            return (true, "Cập nhật sản phẩm thành công", SanPhamBuilder.ToResponse(update!));
        }
        public async Task<(bool, string)> DeleteAsync(string masp)
        {
            var exist = await _repo.GetByMaSP(masp);
            if (exist == null)
            {
                return (false, $"Không tìm thấy sản phẩm có mã {masp}");
            }
            var delete = await _repo.DeleteAsync(masp);
            if (!delete)
            {
                return (false, "Không thể xóa sản phẩm đã có trong đơn hàng");
            }
            return (true, "Xoá sản phẩm thành công");
        }
    }
}
