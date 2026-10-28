using DACN_H_P.Dtos.Request;
using DACN_H_P.Dtos.Response;
using DACN_H_P.Helper;
using DACN_H_P.Mapper;
using DACN_H_P.Model;
using DACN_H_P.Repository;

namespace DACN_H_P.Service.Impl
{
    public class AnhService : IAnhService
    {
        private readonly IAnhRepository _repo;

        private readonly CloudinaryService _cloudinary;
        public AnhService(IAnhRepository repo, CloudinaryService cloudinary)
        {
            _repo = repo;

            _cloudinary = cloudinary;
        }
        public async Task<IEnumerable<AnhResponse>> GetAllAnh()
        {
            var result = await _repo.GetAllAnh();
            return result.Select(AnhBuilder.ToResponse).ToList();
        }
        public async Task<IEnumerable<AnhResponse>> GetByMaSP(string masp)
        {
            var result = await _repo.GetByMaSP(masp);
            if (result == null)
            {
                return null;
            }
            return result.Select(AnhBuilder.ToResponse).ToList();
        }
        public async Task<(bool, string, IEnumerable<AnhResponse>)> PostAsync(AnhRequest request)
        {
            var existSP = await _repo.CheckSP(request.MaSp);
            if (!existSP)
            {
                return (false, "Không có sản phẩm này để thêm ảnh", null);
            }
            var listAnhSP = new List<AnhSp>();
            if (request.AnhSPs != null && request.AnhSPs.Count > 0)
            {
                foreach (var img in request.AnhSPs)
                {
                    var url = await _cloudinary.UploadImageAsync(img);
                    if(!string.IsNullOrEmpty(url))
                    {
                        var data = new AnhSp
                        {
                            MaAnh = Guid.NewGuid().ToString(),
                            MaSp = request.MaSp,
                            DuongDan = url
                        };
                        listAnhSP.Add(data);
                    }
                }
            }
            else
            {
                return (false, "Chưa có ảnh để thêm", null);
            }
            var result = await _repo.PostAsync(listAnhSP);
            return (true, "Thêm ảnh thành công", result.Select(AnhBuilder.ToResponse));
        }
        public async Task<(bool, string)> DeleteAsync(string maanh)
        {
            var result = await _repo.DeleteAsync(maanh);
            if(!result)
            {
                return (false, "Không có ảnh có mã trên");
            }
            return (true, "Xóa ảnh thành công");
        }
    }
}
