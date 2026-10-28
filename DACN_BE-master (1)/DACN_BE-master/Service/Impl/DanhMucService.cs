using DACN_H_P.Dtos.Request;
using DACN_H_P.Dtos.Response;
using DACN_H_P.Model;
using DACN_H_P.Repository;
using DACN_H_P.Mapper;

namespace DACN_H_P.Service.Impl
{
    public class DanhMucService:IDanhMucService
    {
        private readonly IDanhMucRepository _repo;

        public DanhMucService(IDanhMucRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<DanhMucResponse>> GetAllAsync()
        {
            var result =  await _repo.GetAllAsync();
            return result.Select(DanhMucBuilder.ToResponse).ToList();
        }
        public async Task<DanhMucResponse> GetByMaDMAsync(string madm)
        {
            var exist = await _repo.GetByMaDMAsync(madm);
            if(exist == null)
            {
                return null;
            }
            return DanhMucBuilder.ToResponse(exist);
        }
        public async Task<IEnumerable<DanhMucResponse>> GetByTenDMAsync(string tendm)
        {
            var result = await _repo.GetByTenDMAsync(tendm);
            if(result == null)
            {
                return null;
            }
            return result.Select(DanhMucBuilder.ToResponse).ToList();
        }
        public async Task<(bool, string, DanhMucResponse)> PostAsync(DanhMucRequest request)
        {
            var exist = await _repo.GetByMaDMAsync(request.MaDanhMuc);
            if(exist != null)
            {
                return (false, "Danh mục đã tồn tại", null);
            }

            var newDanhMuc = new DanhMuc
            {
                MaDanhMuc = request.MaDanhMuc,
                TenDanhMuc = request.TenDanhMuc,
                MoTa = request.MoTa
            };
            var data = await _repo.PostAsync(newDanhMuc);
            return (true, "Thêm danh mục thành công", DanhMucBuilder.ToResponse(data));
        }
        public async Task<(bool, string, DanhMucResponse)> EditAsync(DanhMucRequest request)
        {
            var exist = await _repo.GetByMaDMAsync(request.MaDanhMuc);
            if(exist == null)
            {
                return (false, "Danh mục không tồn tại", null);
            }
            exist.TenDanhMuc = request.TenDanhMuc;
            exist.MoTa = request.MoTa;
            var data = await _repo.EditAsync(exist);
            return (true, "Sửa thành công", DanhMucBuilder.ToResponse(data));
        }
        public async Task<(bool, string)> DeleteAsync(string madm)
        {
            var exist = await _repo.GetByMaDMAsync(madm);
            if( exist == null)
            {
                return (false, "Danh mục không tồn tại");
            }
            var delete = await _repo.DeleteAsync(madm);
            if (!delete)
            {
                return (false, "Không thể xóa danh mục đang có sản phẩm");
            }
            return (true, "Xóa danh mục thành công");
            
        }
    }
}
